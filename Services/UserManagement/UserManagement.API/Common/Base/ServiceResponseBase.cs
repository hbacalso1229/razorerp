using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace UserManagement.API.Common.Base
{
    /// <summary>
    /// Service Response Type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResponseType
    {
        /// <summary>
        /// Info Type
        /// </summary>
        Info,

        /// <summary>
        /// Warning Type
        /// </summary>
        Warning,

        /// <summary>
        /// Error Type
        /// </summary>
        Error
    }

    /// <summary>
    /// Service response
    /// </summary>
    public abstract class ServiceResponseBase
    {
        public ServiceResponseBase()
        {
            ResponseInfo = new ResponseInfo();
        }

        /// <summary>
        /// Data object
        /// </summary>
        public object Data { get; set; }

        public ResponseInfo ResponseInfo { get; set; }
        public bool Success { get; set; }
    }

    public sealed class ResponseInfo
    {
        public ResponseInfo(ResponseType responseType = ResponseType.Info, string statuscode = "200", string source = "", string descrption = "Success")
        {
            this.ResponseType = ResponseType;
            this.Code = statuscode;
            this.Source = String.IsNullOrEmpty(source) ? $"{Assembly.GetExecutingAssembly().GetName().Name}: {MethodBase.GetCurrentMethod().Name}" : source;
            this.Description = descrption;
        }

        [DefaultValue("")]
        public string Code { get; private set; }

        [DefaultValue("")]
        public string Description { get; private set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(ResponseType))]
        public ResponseType ResponseType { get; private set; }
        [DefaultValue("")]
        public string Source { get; private set; }
    }
}
