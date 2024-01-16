using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ways
{
    FOWARD,
    BACKWARD,
}

public class CubeMovement : MonoBehaviour
{


    public float SpeedCharge;//Speed charge to calculate sliding effect


    public float CameraRotatioRate = 115; // camera rotation degrees per second

    Ways PlayerMovementWay = Ways.FOWARD;


    private Quaternion targetOrientation;


    // Start is called before the first frame update
    void Start()
    {

        // Moves an object to the set position
        transform.position = new Vector3(0, 0, 0);

        targetOrientation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerRelativeToCamera();
        //if (Input.GetKey(MoveForward) && !Input.GetKey(MoveBackward) && !Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(0,0,MovementSpeed);
        //}

        //else if (Input.GetKey(MoveBackward) && !Input.GetKey(MoveForward) && !Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -MovementSpeed);
        //}


        //else if (Input.GetKey(MoveLeft) && !Input.GetKey(MoveBackward) && !Input.GetKey(MoveForward) && !Input.GetKey(MoveRight))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(-MovementSpeed, 0, 0);
        //}


        //else if (Input.GetKey(MoveRight) && !Input.GetKey(MoveBackward) && !Input.GetKey(MoveLeft) && !Input.GetKey(MoveForward))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(+MovementSpeed, 0, 0);
        //}


        //else if (Input.GetKey(MoveLeft) && Input.GetKey(MoveForward) && !Input.GetKey(MoveBackward) && !Input.GetKey(MoveRight))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(-MovementSpeed, 0, MovementSpeed);
        //}

        //else if (Input.GetKey(MoveRight) && Input.GetKey(MoveForward) && !Input.GetKey(MoveLeft) && !Input.GetKey(MoveBackward))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(MovementSpeed, 0, MovementSpeed);
        //}

        //else if (Input.GetKey(MoveLeft) && Input.GetKey(MoveBackward) && !Input.GetKey(MoveForward) && !Input.GetKey(MoveRight))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(-MovementSpeed, 0, -MovementSpeed);
        //}

        //else if (Input.GetKey(MoveRight) && Input.GetKey(MoveBackward) && !Input.GetKey(MoveForward) && !Input.GetKey(MoveLeft))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(MovementSpeed, 0, -MovementSpeed);
        //}
        //else if (!Input.GetKey(MoveRight) && !Input.GetKey(MoveBackward) && !Input.GetKey(MoveForward) && !Input.GetKey(MoveLeft))
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //}


        //Rotqzione della camera(attualmente si attiva col right click del mouse, volendo si puÚ tenere sempre attiva o cambiare metodo di attivazione)
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, CameraRotatioRate * Time.deltaTime);
            targetOrientation = Quaternion.LookRotation(transform.right);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, CameraRotatioRate * Time.deltaTime);
            targetOrientation = Quaternion.LookRotation(-transform.right);
        }


    }

    //Main function for camera-related movement
    void MovePlayerRelativeToCamera()
    {
        //Get Player input(just vertical Axis and Horizontal Axis for now)
        float PlayerVerticalInput = 0;
        float PlayerHorizontalInput = 0;



        //SLIDE EFFECT(INERTIA)
        if (PlayerMovementWay == Ways.FOWARD)
        {
            PlayerVerticalInput = SpeedCharge;
            PlayerHorizontalInput = SpeedCharge;
        }
        else
        {
            PlayerVerticalInput = -SpeedCharge;
            PlayerHorizontalInput = -SpeedCharge;
        }



        //GET INPUT AXIS
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //IF ALREADY GOING FOWARD
            if (PlayerMovementWay == Ways.FOWARD)
            {
                PlayerVerticalInput = Input.GetAxis("Vertical") * SpeedCharge;
                PlayerHorizontalInput += Input.GetAxis("Horizontal") * SpeedCharge;
            }
            //IF NOT YET GOING FOWARD(SLIDING FROM PREVIOUS INPUT)
            else
            {
                if (SpeedCharge == 0)
                {
                    PlayerMovementWay = Ways.FOWARD;
                }
                else
                {
                    //BRAKING
                    SpeedCharge = Mathf.Max(0, SpeedCharge - 0.0065f);
                }
            }

            //IF ALREADY GOING FOWARD
            if (PlayerMovementWay == Ways.FOWARD)
            {
                SpeedCharge = Mathf.Min(2, SpeedCharge + 0.005f);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //IF ALREADY GOING BACKEÏWARD
            if (PlayerMovementWay == Ways.BACKWARD)
            {
                PlayerVerticalInput = Input.GetAxis("Vertical") * SpeedCharge;
                PlayerHorizontalInput = Input.GetAxis("Horizontal") * SpeedCharge;
            }
            //IF NOT YET GOING BACKWARD
            else
            {
                if (SpeedCharge == 0)
                {
                    PlayerMovementWay = Ways.BACKWARD;
                }
                else
                {
                    //BRAKING
                    SpeedCharge = Mathf.Max(0, SpeedCharge - 0.0065f);
                }
            }
            //IF ALREADY GOING BACKEÏWARD
            if (PlayerMovementWay == Ways.BACKWARD)
            {

                SpeedCharge = Mathf.Min(2, SpeedCharge + 0.005f);
            }
        }
        else
        {
            SpeedCharge = Mathf.Max(0, SpeedCharge - 0.0015f);
        }

        //Get Camera's Normalized Directional Vectors
        Vector3 Forward = Camera.main.transform.forward;
        Vector3 Right = Camera.main.transform.right;

        Forward.y = 0;
        Right.y = 0;


        //Normalizzazione (normalizza i vettori a modulo 1)
        Forward = Forward.normalized;
        Right = Right.normalized;

        //Create direction-relative-input vectors
        Vector3 ForwardRelativeVerticalInput = PlayerVerticalInput * Forward;
        Vector3 RightRelativeHorizontalInput = PlayerVerticalInput * Right;

        //Create and apply camera movement
        Vector3 CameraRelatedMovement = ForwardRelativeVerticalInput + RightRelativeHorizontalInput;
        this.transform.Translate(CameraRelatedMovement, Space.World);

    }
}
