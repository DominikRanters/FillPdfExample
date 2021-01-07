using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            FillForm();
            using (var template = new FileStream( @"..\..\..\PDFs\Template.pdf",
                FileMode.Open))
            {
                using (var original = new FileStream(@"..\..\..\PDFs\Original.pdf",
                    FileMode.Open))
                {
                    CopyForm(original, template);
                }
            }
        }

        public static MemoryStream FillForm(Stream inputStream)
        {
            var outStream = new MemoryStream();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            Stream inStream = null;
            try
            {
                pdfReader = new PdfReader(inputStream);
                pdfStamper = new PdfStamper(pdfReader, outStream);
                var form = pdfStamper.AcroFields;

                // foreach (var key in keys) Console.WriteLine(key);
                form.SetFieldProperty(PdfKeys.document_type, "textcolor", new BaseColor(120, 120, 120), null);
                form.SetFieldProperty(PdfKeys.document_type, "textsize", 14f, null);
                form.SetField(PdfKeys.document_type, PdfValues.Original);
              
                // form.Fields[PdfKeys.document_type].
                
                form.SetField(PdfKeys.q_fever_false, PdfValues.True);
                form.SetField(PdfKeys.q_cronic_immuno_true, PdfValues.True);
                form.SetField(PdfKeys.q_cronic_immuno_text, "Essstörung");
                form.SetField(PdfKeys.q_blood_false, PdfValues.True);
                form.SetField(PdfKeys.q_allergy_false, PdfValues.True);
                form.SetField(PdfKeys.q_allergy_reaction_false, PdfValues.True);
                form.SetField(PdfKeys.q_14days_true, PdfValues.True);
                form.SetField(PdfKeys.birthdate, "22.03.2000");
                form.SetField(PdfKeys.address, "Wessumer Straße 2, 48683 Ahaus");
                form.SetField(PdfKeys.q_consent1, PdfValues.True);
                form.SetField(PdfKeys.q_consent2, PdfValues.True);
                form.SetField(PdfKeys.q_consent4, PdfValues.True);
                form.SetField(PdfKeys.name, "Ranters, Dominik");
                
                form.GenerateAppearances = true;
                pdfStamper.FormFlattening = true;
                return outStream;
            }
            finally
            {
                pdfStamper?.Close();
                pdfReader?.Close();
                inStream?.Close();
            }
        }

        public static void CopyForm(Stream original, Stream template)
        {
            PdfReader orgReader = new PdfReader(original); // a document without a form
            PdfReader outReader = new PdfReader(template); // a document with a form
            PdfCopyForms copy = new PdfCopyForms(new FileStream(@"..\..\..\PDFs\Original_Copy.pdf", FileMode.Create));
            copy.AddDocument(orgReader);
            copy.CopyDocumentFields(outReader);
            copy.Close();
            orgReader.Close();
            outReader.Close();
        }

        public static void FillForm()
        {
            byte[] newBuffer;
            using (Stream pdfInputStream =
                new FileStream(@"..\..\..\PDFs\Template.pdf",
                    FileMode.Open))
            using (var resultPDFStream = FillForm(pdfInputStream))
            {
                newBuffer = resultPDFStream.ToArray();
            }

            File.WriteAllBytes(@"..\..\..\PDFs\Filled_Copy.pdf", newBuffer);
        }


        private class PdfKeys
        {
            public static string q_cronic_immuno_text = "q_cronic_immuno_text";
            public static string q_allergy_text = "q_allergy_text";
            public static string q_allergy_reaction_text = "q_allergy_reaction_text";
            public static string name = "name";
            public static string birthdate = "birthdate";
            public static string address = "address";
            public static string consent_place_date = "consent_place_date";
            public static string signature_patient = "signature_patient";
            public static string signature_doctor = "signature_doctor";
            public static string q_fever_true = "q_fever_true";
            public static string q_fever_false = "q_fever_false";
            public static string q_cronic_immuno_true = "q_cronic_immuno_true";
            public static string q_cronic_immuno_false = "q_cronic_immuno_false";
            public static string q_blood_true = "q_blood_true";
            public static string q_blood_false = "q_blood_false";
            public static string q_allergy_true = "q_allergy_true";
            public static string q_allergy_false = "q_allergy_false";
            public static string q_allergy_reaction_true = "q_allergy_reaction_true";
            public static string q_allergy_reaction_false = "q_allergy_reaction_false";
            public static string q_pregnant_true = "q_pregnant_true";
            public static string q_pregnant_false = "q_pregnant_false";
            public static string q_14days_true = "q_14days_true";
            public static string q_14days_false = "q_14days_false";
            public static string q_consent1 = "q_consent1";
            public static string q_consent2 = "q_consent2";
            public static string q_consent3 = "q_consent3";
            public static string q_consent4 = "q_consent4";
            public static string document_type = "document_type";
        }
        
        
        private class PdfValues
        {
            public static string True = "Yes";
            public static string Original = "ORIGINAL";
            public static string Duplicate = "DUPLIKAT";
        }
        
    }
}