# customUssdValidationApi
This is a Sample C-sharp code for verifying the signature that comes from Yo Custom USSD Application at the validation stage. If you have your USSD application handled by Yo! and at some stage during user interaction, there is a step that requires validation of user's details, then you can use this sample code to learn how to verify the signature which comes in the validation request.

In this code, I assume you already know how to handle the HTTP request i.e. You have already create an HTTP end-point which receives the validation request details (such as the anumbermsisdn, datetime, signature and other fields as may be configured), and all you need is to validate the request through signature verification. 

Requirements
===========================================
- Target .NET 4.5.2.

How to Test
===========================================
Open the project in your Visual Studio IDE, then run the debug. You should get a message printed on the console - indicating that the verificaiton was successful.

How to Use the Source Code
=============================================
There is a sub class under Utils class which represents the data extracted out of the HTTP request that comes from Yo USSD application. Depending on the configuration, if your account was configured at Yo side to receive HTTP JSON in the request Payload, then you need to parse this JSON to extract the necessary field
However, if the request format is a regular HTTP POST, then you need to obtain the required fields and use them to create an object of the UssdValidationData class.

To verify the signature, you will need the JSON data and the path to the public key file. The public key file can be found in the debug folder of this project. It's saved as public_key.pem. Then you will need to instanciate an object of Utils and use the verify signature method.
