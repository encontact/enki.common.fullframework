namespace Enki.Common.WebUtils
{
    public class JsonReturn
    {
        public object data { get; set; }
        public bool status { get; set; }

        public JsonReturn() { }

        public JsonReturn(bool status, object data)
        {
            this.status = status;
            this.data = data;
        }
    }
}
