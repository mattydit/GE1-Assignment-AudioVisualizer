using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFlowField))]
public class AudioFlowField : MonoBehaviour
{
    NoiseFlowField noiseFlowfield;
    public AudioPeer audioPeer;

    //Speed
    public bool useSpeed;
    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;

    //Scale
    public bool useScale;
    public Vector2 scaleMinMax;

    //Material
    public Material mat;
    private Material[] audioMat;
    public bool useColour1;
    public bool useColour2;
    public Gradient gradient1;
    public Gradient gradient2;
    private Color[] colour1;
    private Color[] colour2;
    [Range(0f, 1f)]
    public float colourThreshold1;
    public float colourMultiplier1;
    [Range(0f,1f)]
    public float colourThreshold2;
    public float colourMultiplier2;
    public string colourName1;
    public string colourName2;

    // Start is called before the first frame update
    void Start()
    {
        noiseFlowfield = GetComponent<NoiseFlowField>();
        audioMat = new Material[8];
        colour1 = new Color[8];
        colour2 = new Color[8];

        for (int i = 0; i < 8; i++)
        {
            colour1[i] = gradient1.Evaluate((1f / 8f) * i);
            colour2[i] = gradient2.Evaluate((1f / 8f) * i);
            audioMat[i] = new Material(mat);
        }

        int countBand = 0;

        for (int i = 0; i < noiseFlowfield.amountOfParticles; i++)
        {
            int band = countBand % 8;
            noiseFlowfield.particleMeshRenderer[i].material = audioMat[band];
            noiseFlowfield.particles[i].audioBand = band;
            countBand++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(useSpeed)
        {
            noiseFlowfield.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, audioPeer.amplitudeBuffer);
            noiseFlowfield.particleRotSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer.amplitudeBuffer);
        }

        for (int i = 0; i < noiseFlowfield.amountOfParticles; i++)
        {
            if(useScale)
            {
                float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audioPeer.audioBandBuffer[noiseFlowfield.particles[i].audioBand]);
                noiseFlowfield.particles[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        for (int i = 0; i < 8; i++)
        {
        
            if(audioPeer.audioBandBuffer[i] > colourThreshold1)
            {
                audioMat[i].SetColor(colourName1, colour1[i] * audioPeer.audioBandBuffer[i] * colourMultiplier1);
            }
            else
            {
                audioMat[i].SetColor(colourName1, colour1[i] * 0f);
            }
            
            if (audioPeer.audioBand[i] > colourThreshold2)
            {
                audioMat[i].SetColor(colourName2, colour2[i] * audioPeer.audioBand[i] * colourMultiplier2);
            }
            else
            {
                audioMat[i].SetColor(colourName2, colour2[i] * 0f);
            }
            
        }

        noiseFlowfield.cube.transform.Rotate(new Vector3(0, audioPeer.audioBandBuffer[0], 0));
        noiseFlowfield.transform.Rotate(new Vector3(0, audioPeer.audioBandBuffer[0], 0));
    }
}
