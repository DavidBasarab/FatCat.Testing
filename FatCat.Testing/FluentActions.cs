namespace FatCat.Testing;

public static class FluentActions
{
	public static Func<Task> Awaiting(Func<Task> action)
	{
		return action;
	}

	public static Func<Task> Awaiting<T>(this T subject, Func<T, Task> action)
	{
		return () => action(subject);
	}

	public static Action Invoking(Action action)
	{
		return action;
	}

	public static Action Invoking<T>(this T subject, Action<T> action)
	{
		return () => action(subject);
	}
}
