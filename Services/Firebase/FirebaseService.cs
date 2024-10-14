using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASP.NetCore_WhatsApp_1.Services.Firebase
{
    public class FirebaseService
    {
        private readonly FirestoreDb _firestoreDb;

        // Obtengo los parametros desde archivo json para conectarme a Firebase
        public FirebaseService()
        {
            var pathFirebaseAccount = "firebase_config/app-whatsapp-aspnetcore-firebase-adminsdk-owj7t-b5eb481bbb.json";

            // Inicializa la App Firebase
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(pathFirebaseAccount),
            });

            // Leer desde el json
            var json = File.ReadAllText(pathFirebaseAccount);
            var jsonObject = JObject.Parse(json);
            var projectId = jsonObject["project_id"]?.ToString();

            // Inicia Firestore
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        // Guardar el access token recibido en Firestore Database: al...mon@gm
        public async Task SaveDataAsync(string accessToken, object data)
        {
            var docRef = _firestoreDb.Collection("Meta_WA_ASPNetCore").Document(accessToken);

            var token = new
            {
                Token = data,
                CreatedAt = Timestamp.GetCurrentTimestamp()
            };

            await docRef.SetAsync(token);
        }


        // Leer en la coleccion y documento recibido.
        // Obtener el token desde Firestore Database
        public async Task<string> GetAccessTokenAsync(string collectionToRead, string documentToRead)
        {
            try
            {
                // Obtener referencia al documento
                var docRef = _firestoreDb.Collection(collectionToRead).Document(documentToRead);

                // Obtener Documento
                var snapshot = await docRef.GetSnapshotAsync();

                // Verifica que exista el doc
                if (snapshot.Exists)
                {
                    // Obtengo el campo Token desde el documento
                    var tokenMap = snapshot.GetValue<Dictionary<string, object>>("Token");
                    var tokenJsonString = tokenMap["Token"] as string;

                    if (!string.IsNullOrEmpty(tokenJsonString))
                    {
                        // Ya sabemos que existe Token
                        var tokenData = JsonConvert.DeserializeObject<Dictionary<string, object>>(tokenJsonString);
                        
                        if (tokenData != null && tokenData.ContainsKey("access_token"))
                        {
                            return tokenData["access_token"].ToString();
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            // Si hay error o si es null
            return null;
        }



    }
}
