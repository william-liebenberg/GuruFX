namespace GuruFX.Core
{
	public interface IUpdateable : IBaseObject
	{
		/// <summary>
		/// Update the Current State of the concrete. 
		/// </summary>
		/// <param name="elapsedTime">Total Elapsed Time since the first update</param>
		/// <param name="deltaTime">Time passed since last Update</param>
		void Update(double elapsedTime, double deltaTime);
	}
}
