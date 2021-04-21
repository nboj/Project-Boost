using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour { 
    [SerializeField] Vector3 movementVector;
    [Range(0f, 1f)][SerializeField] float movementFactor;
    [SerializeField] float period = 2f;
    private Vector3 startingPosition;
    void Start() {
        startingPosition = transform.position;
    }
 
    void Update() {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float TAU = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * TAU);
        movementFactor = (rawSinWave + 1f) / 2f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
