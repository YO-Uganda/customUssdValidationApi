using System;

namespace SampMerchantUssdValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            * location the public key file.
            * Obtain the request data - Sometimes it comes as HTTP Post parameters
            * and sometimes it comes as JSON payload. The example below is for the JSON
            * data which came as the request payload and we saved it in sampledata.json file.
            */
            String publicKeyFile = "public_key.pem";//Set this to the path to .pem file
            string text = System.IO.File.ReadAllText(@"sampledata.json");//This is your JSON string
            

            Utils utils = new Utils(publicKeyFile);
            bool verified = utils.verifySignature(text);
            if (!verified)
            {
                //Check why the verification failed
                Console.WriteLine("Error: " + utils.ErrorMessage);
                System.Threading.Thread.Sleep(15000);
                return;
            }

            Console.WriteLine("Signature Verification was successful!");
            System.Threading.Thread.Sleep(15000);
        }
    }
}
