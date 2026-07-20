function Convert-FluentAssertion
{
	<#
	.SYNOPSIS
		Rewrites FluentAssertions negations to the FatCat.Testing `.Not.` shape across a source tree.

	.DESCRIPTION
		Applies the mechanical migration codemod described in MIGRATION.md / gaps.md section 5.3:

			.Should().NotXxx(   ->   .Should().Not.Xxx(

		The transform is idempotent (an already-converted `.Should().Not.Xxx(` is never rewritten again,
		because the regex requires an uppercase letter immediately after `Not`, and a converted call has a
		`.` there instead) and honours -WhatIf through ShouldProcess.

		Cases the single regex cannot safely rewrite are reported to a manual-review list rather than being
		silently skipped:
			- chained negations after `.And.` (`.And` is deferred, gap G13) — hand-rewrite into separate statements
			- line-broken chains where `.Should()` and `.NotXxx(` sit on different lines
			- project-defined method names that begin with `Not`
			- negations on a subject whose FatCat gap has not landed yet (these compile only after the gap ships)

	.PARAMETER Path
		Root directory scanned recursively for source files.

	.PARAMETER Filter
		File-name filter passed to Get-ChildItem. Defaults to '*.cs'.

	.EXAMPLE
		./Migrate-FluentAssertions.ps1 -Path C:\Code\FatCat.Toolkit -WhatIf

	.EXAMPLE
		./Migrate-FluentAssertions.ps1 -Path C:\Code\Fog
	#>
	[CmdletBinding(SupportsShouldProcess = $true)]
	param
	(
		[Parameter(Mandatory = $true)]
		[string] $Path,

		[string] $Filter = '*.cs'
	)

	if (-not (Test-Path -Path $Path))
	{
		throw "Path not found: $Path"
	}

	$negationPattern = [regex] '\.Should\(\)\.Not([A-Z]\w*)\('
	$candidateNotPattern = [regex] '(?<![A-Za-z0-9_])Not[A-Za-z]\w*\('

	$rewrittenFiles = [System.Collections.Generic.List[object]]::new()
	$reviewItems = [System.Collections.Generic.List[object]]::new()

	$files = Get-ChildItem -Path $Path -Filter $Filter -File -Recurse

	foreach ($file in $files)
	{
		$originalContent = Get-Content -Path $file.FullName -Raw

		if ([string]::IsNullOrEmpty($originalContent))
		{
			continue
		}

		$matchCount = $negationPattern.Matches($originalContent).Count

		if ($matchCount -gt 0)
		{
			$updatedContent = $negationPattern.Replace($originalContent, '.Should().Not.$1(')

			$rewrittenFiles.Add([pscustomobject]@{ File = $file.FullName; Rewrites = $matchCount })

			if ($PSCmdlet.ShouldProcess($file.FullName, "Rewrite $matchCount FluentAssertions negation(s) to .Not."))
			{
				Set-Content -Path $file.FullName -Value $updatedContent -NoNewline -Encoding utf8
			}
		}

		$lines = $originalContent -split "`r?`n"

		for ($index = 0; $index -lt $lines.Count; $index++)
		{
			$line = $lines[$index]

			foreach ($match in $candidateNotPattern.Matches($line))
			{
				$prefix = $line.Substring(0, $match.Index)

				if ($prefix.EndsWith('.Should().'))
				{
					continue
				}

				$previousLine = ''

				if ($index -gt 0)
				{
					$previousLine = $lines[$index - 1].TrimEnd()
				}

				$reason = 'Method name begins with Not (project-defined) or negation on a not-yet-landed subject -- verify manually'

				if ($prefix.EndsWith('.And.'))
				{
					$reason = 'Chained negation after .And (.And is deferred, gap G13) -- hand-rewrite into separate statements'
				}
				elseif ($prefix.TrimEnd().EndsWith('.Should()') -or $previousLine.EndsWith('.Should()'))
				{
					$reason = 'Line-broken .Should() / .NotXxx( chain -- rejoin or rewrite manually'
				}

				$reviewItems.Add(
					[pscustomobject]@{
						File = $file.FullName
						Line = $index + 1
						Text = $line.Trim()
						Reason = $reason
					}
				)
			}
		}
	}

	Write-Host ''
	Write-Host 'FluentAssertions -> FatCat.Testing codemod'
	Write-Host "Scanned $($files.Count) file(s) under $Path"
	Write-Host ''

	if ($rewrittenFiles.Count -gt 0)
	{
		$totalRewrites = ($rewrittenFiles | Measure-Object -Property Rewrites -Sum).Sum

		Write-Host 'Automatic rewrites  (.Should().NotXxx(  ->  .Should().Not.Xxx():'

		foreach ($rewrittenFile in $rewrittenFiles)
		{
			Write-Host "  $($rewrittenFile.Rewrites)x  $($rewrittenFile.File)"
		}

		Write-Host ''
		Write-Host "  Total: $totalRewrites rewrite(s) across $($rewrittenFiles.Count) file(s)."

		if ($WhatIfPreference)
		{
			Write-Host '  -WhatIf: no files were modified.'
		}
	}
	else
	{
		Write-Host 'Automatic rewrites: none found.'
	}

	Write-Host ''

	if ($reviewItems.Count -gt 0)
	{
		Write-Host "Manual review required  ($($reviewItems.Count) case(s), NOT auto-changed):"

		foreach ($reviewItem in $reviewItems)
		{
			Write-Host "  $($reviewItem.File):$($reviewItem.Line)  [$($reviewItem.Reason)]"
			Write-Host "      $($reviewItem.Text)"
		}
	}
	else
	{
		Write-Host 'Manual review required: none detected.'
	}

	Write-Host ''
}

if ($MyInvocation.InvocationName -ne '.')
{
	Convert-FluentAssertion @args
}
