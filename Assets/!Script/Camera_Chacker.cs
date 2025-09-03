using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Chacker : MonoBehaviour
{
    enum Mode
    {
        None,
        Render,
        RenderOut,
    }

    private Mode _mode;

    // Start is called before the first frame update
    void Start()
    {
        _mode = Mode.None;
    }

    // Update is called once per frame
    void Update()
    {
        _Dead();
    }

    private void OnWillRenderObject()
    {
        if (Camera.current.name == "Main Camera")
        {
            _mode = Mode.Render;
        }
    }

    private void _Dead()
    {
        // ÉJÉÅÉâÇ…é Ç¡ÇƒÇ»ÇØÇÍÇŒè¡Ç∑
        if (_mode == Mode.RenderOut)
        {
            Destroy(gameObject);
        }
        else if (_mode == Mode.Render)
        {          
            _mode = Mode.RenderOut;
        }
       
    }
}



