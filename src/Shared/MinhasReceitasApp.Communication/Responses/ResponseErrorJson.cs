namespace MinhasReceitasApp.Communication.Responses;

public class ResponseErrorJson
{
    public IList<string> Errors { get; set; }
    public bool TokenIsExpiret { get; set; }

    public ResponseErrorJson(IList<string> errors) => Errors = errors;

    public ResponseErrorJson(string error) 
    {
        Errors = new List<string>
        {
            error 
        };
    }
}
