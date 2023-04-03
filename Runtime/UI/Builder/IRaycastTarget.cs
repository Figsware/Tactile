namespace Tactile.UI.Builder
{
    public interface IRaycastTarget
    {
        public bool? RaycastEnabled { get; set; }
    }

    public static class RaycastTargetUtil
    {
        public static View<TMessage, TParameters> RaycastTarget<TMessage, TParameters>(this View<TMessage, TParameters> view, bool raycast) where TMessage : Message where TParameters : struct, IViewParameters, IRaycastTarget
        {
            view.ViewParameters.RaycastEnabled = raycast;
            return view;
        } 
        
        public static View<TMessage, TParameters> EnableRaycast<TMessage, TParameters>(this View<TMessage, TParameters> view) where TMessage : Message where TParameters : struct, IViewParameters, IRaycastTarget
        {
            view.ViewParameters.RaycastEnabled = true;
            return view;
        }
        
        public static View<TMessage, TParameters> DisableRaycast<TMessage, TParameters>(this View<TMessage, TParameters> view) where TMessage : Message where TParameters : struct, IViewParameters, IRaycastTarget
        {
            view.ViewParameters.RaycastEnabled = false;
            return view;
        }
    }
}