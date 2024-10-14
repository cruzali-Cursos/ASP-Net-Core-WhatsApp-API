namespace ASP.NetCore_WhatsApp_1.Util
{
    public class Util : IUtil
    {
        public object TextMessage(string message, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "text",
                text = new
                {
                    body = message
                }
            };
        }

        public object ImageMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "image",
                image = new
                {
                    link = url
                }
            };
        }

        public object AudioMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "audio",
                audio = new
                {
                    link = url
                }
            };
        }

        public object VideoMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "video",
                video = new
                {
                    link = url
                }
            };
        }

        public object DocumentMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "document",
                document = new
                {
                    link = url
                }
            };
        }

        public object LocationMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "location",
                location = new
                {
                    latitude = "-12.067079752918158",
                    longitude = "-77.03371847563524",
                    name = "Estado ubicacion",
                    address = "La direccion es..."
                }
            };
        }


        public object ButtonsMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new
                    {
                        text = "Seleccione una opcion boton"
                    },
                    action = new
                    {
                        buttons = new List<object>
                        {
                            // Hasta 3 botones máximo
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "01",
                                    title = "Accion1"
                                }
                            },
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "02",
                                    title = "Accion2"
                                }
                            }

                        }
                    }
                }
            };
        }


    }
}
