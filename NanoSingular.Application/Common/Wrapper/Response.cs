// response wrapper classes

namespace NanoSingular.Application.Common.Wrapper
{
    public class Response
    {

        public bool Succeeded { get; set; }
        public List<string> Messages { get; set; } = new();

        public static Response Success()
        {
            return new Response { Succeeded = true };
        }

        public static Response Fail() //fail and no messages
        {
            return new Response { Succeeded = false };
        }

        public static Response Fail(string message)  //fail and singular message
        {
            return new Response { Succeeded = false, Messages = new List<string> { message } };
        }

        public static Response Fail(List<string> messages) //fail and list of messages
        {
            return new Response { Succeeded = false, Messages = messages };
        }


    }


    public class Response<T> : Response
    {

        public T Data { get; set; }


        public new static Response<T> Success()
        {
            return new Response<T> { Succeeded = true };
        }


        public static Response<T> Success(T data)
        {
            return new Response<T> { Succeeded = true, Data = data };
        }

        public new static Response<T> Fail()
        {
            return new Response<T> { Succeeded = false };
        }

        public new static Response<T> Fail(string message)
        {
            return new Response<T> { Succeeded = false, Messages = new List<string> { message } };
        }


        public new static Response<T> Fail(List<string> messages)
        {
            return new Response<T> { Succeeded = false, Messages = messages };
        }

    }

}



