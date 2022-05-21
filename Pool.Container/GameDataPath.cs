using System.IO;

namespace Pool.Container;

public class GameDataPath
	: IOrder<string>
{
	private const string FolderPath = @"C:\Tests\Game";
	private const int DataIndex = 0;
	private readonly string[] _fileNames = new string[]
	{
		"GameShapes.xml"
		, "LineCollisionTest.xml"
	};

	public string Order()
	{
		var fileName = _fileNames[DataIndex];
		var filePath = Path.Combine(FolderPath, fileName);
		return filePath;
	}
}