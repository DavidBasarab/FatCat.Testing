<#
.SYNOPSIS
	Rewrites FluentAssertions prefixed negations to the FatCat.Testing 'Not' property form.

.DESCRIPTION
	FluentAssertions spells negation as a prefixed method (NotBeNull, NotContain). FatCat.Testing
	spells it as a 'Not' property followed by the positive method (Not.BeNull, Not.Contain). This
	codemod applies the single mechanical rule '.Should().NotXxx(' -> '.Should().Not.Xxx(' across a
	directory tree of C# source, and reports the four situations the rule cannot safely rewrite.

	The transform is idempotent: an already-migrated '.Should().Not.Xxx(' does not match the find
	pattern, so a second run over the same tree changes nothing.

	See MIGRATION.md section 7 for the full description, the run sequence, and the manual fixes.

.PARAMETER Path
	The root directory to search. Every '*.cs' file beneath it is scanned; 'bin' and 'obj' folders
	are excluded.

.PARAMETER WhatIf
	Report the changes that would be made without writing any file.

.EXAMPLE
	. $PROFILE; ./tools/Convert-FluentAssertions.ps1 -Path ./MyProject -WhatIf
	./tools/Convert-FluentAssertions.ps1 -Path ./MyProject
#>
function Convert-FluentAssertions
{
	param(
		[Parameter(Mandatory = $true)]
		[string]$Path,

		[switch]$WhatIf
	)

	function Write-ReportCases
	{
		param(
			[System.Collections.Generic.List[string]]$Cases
		)

		if ($null -eq $Cases -or $Cases.Count -eq 0)
		{
			Write-Host '   (none found)'

			return
		}

		foreach ($case in $Cases) { Write-Host "   $case" }
	}

	$resolvedPath = Resolve-Path -LiteralPath $Path

	$negationPattern = '\.Should\(\)\.Not([A-Z]\w*)\('
	$chainedNegationPattern = '\.And\.Not([A-Z]\w*)\('
	$lineBrokenNegationPattern = '^\s*\.Not([A-Z]\w*)\('

	$knownNegations = [System.Collections.Generic.HashSet[string]]::new(
		[string[]]@(
			'Be',
			'BeApproximately',
			'BeAssignableTo',
			'BeDefined',
			'BeEmpty',
			'BeEquivalentTo',
			'BeFalse',
			'BeGreaterThan',
			'BeGreaterThanOrEqualTo',
			'BeInAscendingOrder',
			'BeInDescendingOrder',
			'BeInRange',
			'BeLessThan',
			'BeLessThanOrEqualTo',
			'BeNaN',
			'BeNegative',
			'BeNull',
			'BeNullOrEmpty',
			'BeNullOrWhiteSpace',
			'BeOfType',
			'BeOneOf',
			'BePositive',
			'BeSameAs',
			'BeSubsetOf',
			'BeTrue',
			'BeZero',
			'Contain',
			'ContainAll',
			'ContainAny',
			'ContainEquivalentOf',
			'ContainInOrder',
			'ContainMatch',
			'ContainNulls',
			'ContainSingle',
			'EndWith',
			'Equal',
			'HaveCount',
			'HaveElementAt',
			'HaveFlag',
			'HaveSameCount',
			'HaveValue',
			'IntersectWith',
			'Match',
			'MatchRegex',
			'OnlyContain',
			'OnlyHaveUniqueItems',
			'StartWith'
		)
	)

	$rewriteKnownNegation = {
		param([System.Text.RegularExpressions.Match]$match)

		$methodName = $match.Groups[1].Value

		if ($knownNegations.Contains($methodName)) { return ".Should().Not.$methodName(" }

		return $match.Value
	}

	$chainedNegations = [System.Collections.Generic.List[string]]::new()
	$lineBrokenNegations = [System.Collections.Generic.List[string]]::new()
	$projectDefinedNegations = [System.Collections.Generic.List[string]]::new()
	$rewrittenFiles = [System.Collections.Generic.List[string]]::new()

	$sourceFiles = @(
		Get-ChildItem -LiteralPath $resolvedPath -Recurse -Filter '*.cs' -File |
			Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' }
	)

	$totalRewrites = 0

	foreach ($sourceFile in $sourceFiles)
	{
		$relativePath = [System.IO.Path]::GetRelativePath($resolvedPath, $sourceFile.FullName)
		$content = Get-Content -Raw -LiteralPath $sourceFile.FullName

		if ($null -eq $content) { continue }

		$fileRewrites = [System.Text.RegularExpressions.Regex]::Matches($content, $negationPattern) |
			Where-Object { $knownNegations.Contains($_.Groups[1].Value) } |
			Measure-Object |
			Select-Object -ExpandProperty Count

		$lineNumber = 0

		foreach ($line in ($content -split "\r?\n"))
		{
			$lineNumber++

			if ($line -cmatch $chainedNegationPattern)
			{
				$chainedNegations.Add("$($relativePath):$($lineNumber): $($line.Trim())")
			}

			if ($line -cmatch $lineBrokenNegationPattern)
			{
				$lineBrokenNegations.Add("$($relativePath):$($lineNumber): $($line.Trim())")
			}

			foreach ($negation in [System.Text.RegularExpressions.Regex]::Matches($line, $negationPattern))
			{
				$methodName = $negation.Groups[1].Value

				if (-not $knownNegations.Contains($methodName))
				{
					$projectDefinedNegations.Add("$($relativePath):$($lineNumber): $($line.Trim()) [Not$methodName]")
				}
			}
		}

		if ($fileRewrites -eq 0) { continue }

		$totalRewrites += $fileRewrites
		$rewrittenFiles.Add("$($relativePath): $fileRewrites change(s)")

		if ($WhatIf) { continue }

		$rewritten = [System.Text.RegularExpressions.Regex]::Replace($content, $negationPattern, $rewriteKnownNegation)

		Set-Content -LiteralPath $sourceFile.FullName -Value $rewritten -NoNewline
	}

	$rewriteVerb = if ($WhatIf) { 'Would rewrite' } else { 'Rewrote' }

	Write-Host 'Convert-FluentAssertions'
	Write-Host "Scanned $($sourceFiles.Count) file(s) under $resolvedPath."

	if ($WhatIf) { Write-Host 'WhatIf mode — no files were modified.' }

	Write-Host ''
	Write-Host "$rewriteVerb $totalRewrites negation call(s):"

	foreach ($rewrittenFile in $rewrittenFiles) { Write-Host "  $rewrittenFile" }

	if ($rewrittenFiles.Count -eq 0) { Write-Host '  (none)' }

	Write-Host ''
	Write-Host 'Manual review required — the four cases the regex cannot safely rewrite:'
	Write-Host ''

	Write-Host "1. Chained negations through '.And' — '.And' is not supported; split into separate statements:"
	Write-ReportCases -Cases $chainedNegations

	Write-Host ''
	Write-Host "2. Line-broken chains — '.Should()' and '.NotXxx(' on different lines; rewrite by hand:"
	Write-ReportCases -Cases $lineBrokenNegations

	Write-Host ''
	Write-Host '3. Possible project-defined negations — the method is not a known FluentAssertions negation,'
	Write-Host '   so it was left untouched; verify each before rewriting:'
	Write-ReportCases -Cases $projectDefinedNegations

	Write-Host ''
	Write-Host '4. Negations on a subject type the target build does not yet support:'
	Write-Host '   Not detectable from source, and not applicable to these in-repo fixtures. In a consumer'
	Write-Host "   repo a rewritten '.Should().Not.Xxx(' compiles only once FatCat.Testing ships the matching"
	Write-Host '   assertion for that subject type — follow the per-repo order in MIGRATION.md section 5.4.'
}

Convert-FluentAssertions @args
