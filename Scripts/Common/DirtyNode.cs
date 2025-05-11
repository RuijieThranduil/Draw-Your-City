
////////////////////////////////////////////////////////////////////////////////////////////////
//DirtyNode 脏标记节点，用于更新界面
//创建者：LXR
////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class DirtyNode : MonoBehaviour
{
    protected bool mDirty = true;

    public virtual void Dirty(bool isRightNow)
    {
        mDirty = true;
        if (isRightNow)
        {
            OnReset();
            mDirty = false;
        }
    }
    protected virtual void OnReset()
    {

    }

    #region Unity Call Function
    /*当一个脚本实例被载入时Awake被调用。
     * Awake用于在游戏开始之前初始化变量或游戏状态。
     * 在脚本整个生命周期内它仅被调用一次.Awake在所有对象被初始化之后调用，
     * 所以你可以安全的与其他对象对话或用诸如 GameObject.FindWithTag 这样的函数搜索它们。
     * 每个游戏物体上的Awke以随机的顺序被调用。因此，你应该用Awake来设置脚本间的引用，
     * 并用Start来传递信息。Awake总是在Start之前被调用。它不能用来执行协同程序。
     */
    //protected virtual void Awake(){}

    //This function is called when the object becomes enabled and active.
    //protected virtual void OnEnable()
    //{
    //    //MPEventManager.Instance.AddEventHandlerOfObj(this, new MPEventManager.OnEventHandler(OnMPEventHandler));
    //}

    /*Start仅在Update函数第一次被调用前调用。
     * Start在behaviour的生命周期中只被调用一次。
     * 它和Awake的不同是Start只在脚本实例被启用时调用。
     * 你可以按需调整延迟初始化代码。
     * Awake总是在Start之前执行。这允许你协调初始化顺序。
     */
    //protected virtual void Start(){}

    //固定时间刷新
    //protected virtual void FixedUpdate(){}

    //触发器接触开始
    //protected virtual void OnTriggerEnter(Collider other){}
    //触发器结束接触
    //protected virtual void OnTriggerExit(Collider other){}
    ////触发器保持接触
    //protected virtual void OnTriggerStay(Collider other){}

    //碰撞盒开始碰撞
    //protected virtual void OnCollisionEnter(Collision collision){}
    //碰撞盒退出碰撞
    //protected virtual void OnCollisionExit(Collision collision){}
    //碰撞盒保持碰撞
    //protected virtual void OnCollisionStay(Collision collision){}

    ////当鼠标上的按钮被按下时触发的事件
    //protected virtual void OnMouseDown(Vector3 mousePosition){}
    ////当用户鼠标拖拽GUI元素或碰撞体时调用
    //protected virtual void OnMouseDrag(Vector3 mousePosition){}
    ////当鼠标进入物体范围时被调用
    //protected virtual void OnMouseEnter(Vector3 mousePosition){}
    ////当鼠标退出时被调用
    //protected virtual void OnMouseExit(Vector3 mousePosition){}
    ////当鼠标移动到某对象的上方时触发的事件
    //protected virtual void OnMouseOver(Vector3 mousePosition){}
    ////当鼠标按键被松开时触发的事件
    //protected virtual void OnMouseUp(Vector3 mousePosition){}

    //protected virtual void Update()
    //{
    //    if (mDirty)
    //    {
    //        OnReset();
    //        mDirty = false;
    //    }
    //}

    //yield return null;
    //yield return WaitForSeconds;
    //yield return WWW;
    //yield return StartCoroutine
    //yield return new WaitForFixedUpdate();
    //WaitForSeconds(1.0f);

    //protected virtual void LateUpdate(){}
    //protected virtual void OnWillRenderObject(){}
    //void OnPreCull(){}
    //protected virtual void OnBecameVisible(){}
    //protected virtual void OnBecameInvisible(){}
    //protected virtual void OnPreRender(){}
    //protected virtual void OnRenderObject(){}
    //protected virtual void OnPostRender(){}
    //protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination){}
    //protected virtual void OnGUI(){}
    //yield return new WaitForEndOfFrame();
    //protected virtual void OnApplicationPause(bool bPause){}

    //protected virtual void OnDisable(){}
    //protected virtual void OnDestroy(){}
    //protected virtual void OnApplicationQuit(){}
    #endregion
}
