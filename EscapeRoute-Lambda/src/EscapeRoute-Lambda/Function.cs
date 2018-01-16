using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using Amazon.S3;
using Amazon.S3.Model;
using JackWFinlay.EscapeRoute;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace EscapeRoute_Lambda
{
    public class Function
    {
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Function(IAmazonS3 s3Client)
        {
            S3Client = s3Client;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Response> FunctionHandler(Request request, ILambdaContext context)
        {
            
            
            string bucketName = request.QueryStringParameters.TryGetValue("bucketName", out string bucketNameExists)
                ? bucketNameExists
                : null;

            string objectKey = request.QueryStringParameters.TryGetValue("objectKey", out string objectKeyExists)
                ? objectKeyExists
                : null;

            int statusCode;
            string result = "";
            
            try
            {
                GetObjectResponse fileObject = await S3Client.GetObjectAsync(bucketName, objectKey);

                using (var stream = fileObject.ResponseStream)
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string s3Document = streamReader.ReadToEnd();
                    IEscapeRoute escapeRoute = new EscapeRoute();
                    result = await escapeRoute.ParseStringAsync(s3Document);
                }

                statusCode = 200;

            }
            catch(Exception e)
            {
                context.Logger.LogLine($"Error getting object {objectKey} from bucket {bucketName}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                //throw;
                statusCode = 500;
            }
            
            Response response = new Response(statusCode, "json", result);
            return response;
        }
    }
}
