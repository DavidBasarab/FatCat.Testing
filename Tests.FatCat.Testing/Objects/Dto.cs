namespace Tests.FatCat.Testing.Objects;

public class Dto(string name)
{
	public string Name { get; } = name;

	public override bool Equals(object obj)
	{
		if (obj is not Dto other) { return false; }

		return Name == other.Name;
	}

	public override int GetHashCode()
	{
		return Name == null ? 0 : Name.GetHashCode();
	}
}
