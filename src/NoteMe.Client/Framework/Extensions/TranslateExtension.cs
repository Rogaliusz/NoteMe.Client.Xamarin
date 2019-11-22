using System;
using System.Reflection;
using System.Resources;
using Plugin.Multilingual;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client.Framework.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private static ITranslationService _translationService;

        private static ITranslationService TranslationService
            => _translationService ?? (_translationService = TinyIoCContainer.Current.Resolve<ITranslationService>());
        
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider) 
            => Text == null 
                ? "" 
                : TranslationService.Translate(Text);
    }

    public interface ITranslationService
    {
        string Translate(string text);
    }

    public class TranslationService : ITranslationService
    {
        const string ResourceId = "NoteMe.Client.Resources.AppResources";
 
        private static readonly ResourceManager resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension)
            .GetTypeInfo().Assembly);
        
        public string Translate(string text)
        {
            var ci = CrossMultilingual.Current.CurrentCultureInfo;
            var translation = resmgr.GetString(text, ci) ?? text;

            return translation;
        }
    }
}