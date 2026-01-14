namespace JualIn.App.Mobile.Presentation.Core.Extensions
{
    public static class UiElementExtensions
    {
        extension(Element element)
        {
            public T? GetParentPage<T>() where T : Element
            {
                Element parent = element.Parent;

                while (parent != null)
                {
                    if (parent is T parentPage)
                    {
                        return parentPage;
                    }

                    parent = parent.Parent;
                }

                return null;
            }
        }
    }
}
