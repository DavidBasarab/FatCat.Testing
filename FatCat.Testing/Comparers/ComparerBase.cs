using FatCat.Testing.Exceptions;

namespace FatCat.Testing.Comparers;

public abstract class ComparerBase<TSubject, TComparer>(TSubject subject)
	where TComparer : ComparerBase<TSubject, TComparer>
{
	protected TSubject Subject { get; } = subject;

	public TComparer BeAssignableTo(Type expectedType, string because = null)
	{
		var boxed = (object)Subject;

		if (boxed == null || !expectedType.IsAssignableFrom(boxed.GetType())) { CompareException.New(because ?? $"{FormatSubject()} should be assignable to {expectedType.Name}"); }

		return (TComparer)this;
	}

	public TComparer BeAssignableTo<T>(string because = null) { return BeAssignableTo(typeof(T), because); }

	public TComparer BeOfType(Type expectedType, string because = null)
	{
		var boxed = (object)Subject;

		if (boxed == null || boxed.GetType() != expectedType) { CompareException.New(because ?? $"{FormatSubject()} should be of type {expectedType.Name}"); }

		return (TComparer)this;
	}

	public TComparer BeOfType<T>(string because = null) { return BeOfType(typeof(T), because); }

	public TComparer BeOneOf(IEnumerable<TSubject> values, string because = null)
	{
		var valuesList = values.ToList();

		if (!valuesList.Contains(Subject))
		{
			CompareException.New(
								because ?? $"{FormatSubject()} should be one of [{string.Join(", ", valuesList)}]"
								);
		}

		return (TComparer)this;
	}

	public TComparer BeOneOf(params TSubject[] values) { return BeOneOf((IEnumerable<TSubject>)values); }

	public TComparer Satisfy(Action<TSubject> inspector)
	{
		inspector(Subject);

		return (TComparer)this;
	}

	private string FormatSubject()
	{
		var boxed = (object)Subject;

		return boxed == null ? "null" : $"{boxed}";
	}
}