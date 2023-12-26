using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TT
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Manager Joystick
        static Dictionary<string, JoystickController> joysticks = new Dictionary<string, JoystickController>();

        public static void AddJoystick(JoystickController joystick, bool destroyIfExists = true)
        {
            if (joystick == null)
            {
                Debug.Log("Joystick cannot be null");
                return;
            }

            if (joysticks.ContainsKey(joystick.JoysickID))
            {
                Debug.Log(string.Format("Joystick ID: {0} exists!", joystick.JoysickID));
                if (destroyIfExists)
                {
                    Destroy(joystick.gameObject);
                    Debug.Log("Has destroyed Joystick ID: " + joystick.JoysickID);
                }
            }
            else
            {
                joysticks.Add(joystick.joysickID, joystick);
            }
        }

        public static void RemoveJoystick(JoystickController joystick)
        {
            joysticks.Remove(joystick.joysickID);
        }

        public static JoystickController GetJoystick(string nameType)
        {
            if (joysticks.ContainsKey(nameType)) return joysticks[nameType];
            return null;
        }
        #endregion

        #region Joystick Events
        public enum JoystickEvent { JoyBeginDrag, JoyDrag, JoyEndDrag, }
        protected ObserverEvents<JoystickEvent, JoystickController> _events = new ObserverEvents<JoystickEvent, JoystickController>();
        public ObserverEvents<JoystickEvent, JoystickController> Events => _events;
        #endregion

        [SerializeField] protected Image joystick;
        [SerializeField] protected Transform handle;

        [SerializeField] protected string joysickID;
        protected Vector2 originPos;
        protected Vector2 direction;
        protected float radius;

        public virtual float Radius => radius;
        public virtual string JoysickID => joysickID;
        public virtual Vector3 Direction => direction;

        protected virtual void Awake()
        {
            JoystickController.AddJoystick(this);
        }

        protected virtual void Start()
        {
            originPos = joystick.transform.position;
            RectTransform joyRect = joystick.GetComponent<RectTransform>();

            RectTransform rectCanvas = FindRectOfCanvasInParent(transform.parent);
            float canvasRectLocalScale = rectCanvas ? rectCanvas.localScale.x : 1f;

            radius = (joyRect.rect.width / 2f) * canvasRectLocalScale;
        }

        protected virtual void OnDestroy()
        {
            JoystickController.RemoveJoystick(this);
        }

        protected virtual RectTransform FindRectOfCanvasInParent(Transform inParent)
        {
            if (inParent == null) return null;

            Canvas canvas = inParent.GetComponent<Canvas>();
            if (canvas != null) return canvas.GetComponent<RectTransform>();

            return FindRectOfCanvasInParent(inParent.parent);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            joystick.transform.position = eventData.position;
            _events.Notify(JoystickEvent.JoyBeginDrag, this);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 realDirection = Vector2.ClampMagnitude(eventData.position - (Vector2)joystick.transform.position, radius);

            direction = realDirection.normalized;
            handle.position = (Vector2)joystick.transform.position + realDirection;
            _events.Notify(JoystickEvent.JoyDrag, this);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            joystick.transform.position = originPos;
            handle.localPosition = Vector2.zero;
            _events.Notify(JoystickEvent.JoyEndDrag, this);
            direction = Vector2.zero;
        }
    }
}
