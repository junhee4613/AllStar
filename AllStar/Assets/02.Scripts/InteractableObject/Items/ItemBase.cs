using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IItemBase : MonoBehaviour
{
    public byte itemIndex;
    public ItemTypeEnum type;
    public Transform pressText;
    public abstract void UseItem<T>(ref T changeOriginValue);
    protected virtual void Start()
    {


    }
    protected void OnEnable()
    {
        Start();
    }
    public void SetItemModel(Material mat, Mesh mesh, byte index)
    {
        itemIndex = index;
        this.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = mat;
        this.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    public void OBJPushOnly()
    {
        InteractionWindowClose();
        Managers.Pool.Push(this.gameObject);
    }
    public void InteractionWindowOpen()
    {
        if (pressText == null)
        {
            pressText = Managers.Pool.UIPop(Managers.DataManager.Datas["PressFKey"] as GameObject).transform;
            pressText.position = transform.position + Vector3.up;
        }
    }
    public void InteractionWindowClose()
    {

        if (pressText != null)
        {
            Managers.Pool.UIPush(pressText.gameObject);
            pressText = null;
        }

    }
    public Vector3 startVec;
    public LayerMask targetLayer;
    public float speed = 3f;
    protected float meshSize;
    public float defaultFallHeight = 2;
    public float coefficientOfFriction = 1.6f;
    protected Vector3 randomDir;
    public bool IsToUp = false;
    public Vector3 targetVec;
    // Start is called before the first frame update
    // Update is called once per frame
    public void GetItemBouce()
    {
        startVec = transform.position;
        IsToUp = false;
        meshSize = transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.extents.y;
        randomDir = new Vector3(Random.Range(-0.3f, 0.4f), 0, Random.Range(-0.3f, 0.4f));
        targetVec = Vector3.up * defaultFallHeight;
    }
    void Update()
    {
        if (IsToUp)
        {
            return;
        }
        if (Physics.Raycast(transform.position, Vector3.down, meshSize * 3, targetLayer))
        {
            Debug.Log("asd");
            targetVec = targetVec / coefficientOfFriction;
            speed = 9.8f;
            if (targetVec.y < 0.2)
            {
                IsToUp = true;
            }
        }
        transform.Translate((targetVec * Time.deltaTime) * speed);
        if (Physics.Raycast(transform.position + Vector3.right, Vector3.left, meshSize, targetLayer) || Physics.Raycast(transform.position + Vector3.forward, Vector3.back, meshSize, targetLayer))
        {
            Debug.Log("º®´êÀ½");
        }
        else
        {
            transform.Translate((randomDir * Time.deltaTime));
        }
        speed -= (Time.deltaTime * 19.6f);
        if (speed < -100)
        {
            transform.position = startVec;
            IsToUp = true;
        }
    }
}

