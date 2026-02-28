namespace Common.Common
{
    public class CommonResultResponseDto<T>
    {
        public CommonResultResponseDto() { }
        internal CommonResultResponseDto(bool succeeded, IEnumerable<string> errors, IEnumerable<string> messages, T data, int retValue, bool isExist)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Messages = messages.ToArray();
            IsExist = isExist;
            ReturnId = retValue;
            Data = data;
        }
        public bool Succeeded { get; init; }
        public string[] Errors { get; init; }
        public string[] Messages { get; init; }
        public bool IsExist { get; init; }
        public int ReturnId { get; set; }
        public T Data { get; init; }


        public static CommonResultResponseDto<T> Success(IEnumerable<string> messages, T data, int retValue = 0)
        {
            return new CommonResultResponseDto<T>(true, System.Array.Empty<string>(), messages, data, retValue, false);
        }

        public static CommonResultResponseDto<T> Failure(IEnumerable<string> errors, T data, bool isExist = false)
        {
            return new CommonResultResponseDto<T>(false, errors, System.Array.Empty<string>(), data, 0, isExist);
        }
    }
}
