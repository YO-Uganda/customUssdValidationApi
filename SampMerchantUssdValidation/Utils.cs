using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;


using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Signers;

namespace SampMerchantUssdValidation
{
    class Utils
    {
        String publicKeyFile;
        public String ErrorMessage = "";

        public Utils(String publicKeyFile)
        {
            this.publicKeyFile = publicKeyFile;
        }

        static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public bool verifySignature(String jsonData)
        {
            UssdValidationData validationData = JsonConvert.DeserializeObject<UssdValidationData>(jsonData);
            string signedDataString = validationData.datetime + validationData.anumbermsisdn;
            
            try
            {
                byte[] signature_ = Convert.FromBase64String(validationData.signature);
            
                string signedData = Hash(signedDataString);

                if (!File.Exists(publicKeyFile))
                {
                    this.ErrorMessage = "Verification failed: PublicKey Certificate: "
                        + publicKeyFile + " does not exists";
                    return false;
                }

                string x509Pem = File.ReadAllText(publicKeyFile);
                x509Pem = x509Pem.Replace("-----BEGIN PUBLIC KEY-----", "");
                x509Pem = x509Pem.Replace("-----END PUBLIC KEY-----", "");
                x509Pem = x509Pem.Replace("\n", "");


                var publicKeyContent = Convert.FromBase64String(x509Pem);
                var ppubl = PublicKeyFactory.CreateKey(publicKeyContent);

                RsaDigestSigner signer1 = new RsaDigestSigner(new Sha1Digest());
                var signer = SignerUtilities.GetSigner("SHA1WITHRSA");
                signer.Init(false, ppubl);
                byte[] signedD = Encoding.UTF8.GetBytes(signedData);
                signer.BlockUpdate(signedD, 0, signedD.Length);
                bool verify = signer.VerifySignature(signature_);
                if (!verify)
                {
                    this.ErrorMessage = "Signature verification failed.";
                }
                return verify;
                
            }
            catch (Exception e)
            {
                this.ErrorMessage = "Verification failed: " + e.Message;
                return false;
            }
        }

        class UssdValidationData
        {
            public String datetime { get; set; }
            public String anumbermsisdn { get; set; }
            public String signature { get; set; }

            //You can have other fields as per your settings.
            //public String msisdn { get; set; }
            //public String clientnumber { get; set; }
        }
    }
}
