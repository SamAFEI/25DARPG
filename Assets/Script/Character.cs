using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Character : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody rb { get; private set; }
    public GameObject skeleton { get; private set; }
    public List<SpriteResolver> spriteResolvers { get; private set; } = new List<SpriteResolver>();
    public SpriteLibrary spriteLibrary { get; private set; }
    #endregion

    public SpriteLibraryAsset SLAnomal;
    public SpriteLibraryAsset SLAbreak1;
    public bool IsFacingRight;
    public Vector3 MoveInput;
    public float MoveSpeed;
    public string CurrentAnim;
    public string Anim;

    private void Start()
    {
        MoveSpeed = 5f;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        skeleton = transform.Find("Skeleton").gameObject;
        spriteResolvers = skeleton.GetComponentsInChildren<SpriteResolver>().ToList();
        spriteLibrary = skeleton.GetComponent<SpriteLibrary>();
        Anim = "Idle";
    }
    private void Update()
    {
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.z = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 150, ForceMode.Force);
            //rb.velocity = Vector3.up * 100;
        }
    }

    private void FixedUpdate()
    {
        if (MoveInput.x != 0)
        {
            CheckDirectionToFace(MoveInput.x > 0);
        }
        Vector3 _vector = MoveInput.normalized * MoveSpeed;
        rb.velocity = _vector;
        //rb.AddForce(_vector, ForceMode.Force);
        //Debug.Log(_vector);
        if (_vector == Vector3.zero)
        {
            Anim = "Idle";
        }
        else
        {
            Anim = "Run";
        }
        if (CurrentAnim != Anim)
        {
            anim.Play(Anim);
            CurrentAnim = Anim;
        }
    }
    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SpriteResolver resolver = spriteResolvers.Where(x => x.GetCategory() == "HEAD").FirstOrDefault();
            Debug.Log(resolver.GetCategory());
            if (resolver != null)
            {
                string _label = "45";
                if (resolver.GetLabel() == "45") { _label = "90"; }
                resolver.SetCategoryAndLabel(resolver.GetCategory(), _label);
                Debug.Log(resolver.GetLabel());
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpriteResolver resolver = spriteResolvers.Where(x => x.GetCategory() == "HEAD").FirstOrDefault();
            if (resolver != null)
            {
                SpriteRenderer sprite = resolver.GetComponent<SpriteRenderer>();
                sprite.enabled = !sprite.enabled;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (spriteLibrary.spriteLibraryAsset != SLAnomal)
            {
                spriteLibrary.spriteLibraryAsset = SLAnomal;
            }
            else
            {
                spriteLibrary.spriteLibraryAsset = SLAbreak1;
            }
        }
    }
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }
    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
}
