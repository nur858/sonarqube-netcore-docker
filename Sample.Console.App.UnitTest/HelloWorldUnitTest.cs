using NUnit.Framework;
using Sample.Console.App;
namespace Sample.Console.App.UnitTest
{
	[TestFixture]
	public class HelloWorldUnitTest
	{
		private readonly HelloWorld _helloWorld;
		public HelloWorldUnitTest()
		{
			this._helloWorld = new HelloWorld();
		}
		[Test]
		public void ShouldReturnHelloWorld()
		{
			var result = this._helloWorld.ReturnHelloWorld();
			Assert.AreEqual("Hello World", result);
		}
	}
}

