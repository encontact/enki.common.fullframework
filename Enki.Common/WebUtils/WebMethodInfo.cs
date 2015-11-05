namespace Enki.Common
{
    /// <summary>
    /// Information about a web service operation
    /// </summary>
    public class WebMethodInfo
    {
        private string _name;
        private Parameter[] _inputParameters;
        private Parameter[] _outputParameters;

        /// <summary>
        /// OperationInfo
        /// </summary>
        public WebMethodInfo(string name, Parameter[] inputParameters, Parameter[] outputParameters)
        {
            _name = name;
            _inputParameters = inputParameters;
            _outputParameters = outputParameters;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// InputParameters
        /// </summary>
        public Parameter[] InputParameters
        {
            get { return _inputParameters; }
        }

        /// <summary>
        /// OutputParameters
        /// </summary>
        public Parameter[] OutputParameters
        {
            get { return _outputParameters; }
        }
    }
}