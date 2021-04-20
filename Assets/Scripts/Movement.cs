using UnityEngine;

public class Movement : MonoBehaviour { 
    [SerializeField] float thrustAmount = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem ps;
    private Rigidbody rb; 
    private new AudioSource audio;

    private void Start() {
        rb = GetComponent<Rigidbody>(); 
        audio = GetComponent<AudioSource>();    
    }
 
    private void Update() {
        ProcessRotation();
        ProcessThrust();
    }

    private void ProcessRotation() {
        if (Input.GetKey(KeyCode.A)) {
            ApplyRotation(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D)) {
            ApplyRotation(Vector3.back);
        }
    }

    private void ApplyRotation(Vector3 rotation)
    {
        rb.freezeRotation = true;
        transform.Rotate(rotation * rotationThrust * Time.deltaTime);
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    private void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            rb.AddRelativeForce(Vector3.up * thrustAmount * Time.deltaTime);
            if (!audio.isPlaying) {  
                audio.PlayOneShot(mainEngine);
            }
            if (!ps.isPlaying)
                ps.Play();
        } else {
            audio.Stop(); 
            ps.Stop();
        }
    }
}
