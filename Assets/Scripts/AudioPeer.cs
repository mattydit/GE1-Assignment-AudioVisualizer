using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSrc;
    public static float[] samples = new float[512];
    public static float[] freqBands = new float[8];
    public static float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    float[] freqBandHighest = new float[8];
    public float[] audioBand = new float[8];
    public float[] audioBandBuffer = new float[8];

    public float amplitude, amplitudeBuffer;
    float amplitudeHighest;

    //Mic input
    public AudioClip audioClip;
    public bool useMicrophone;
    public string selectedDevice;
    public UnityEngine.Audio.AudioMixerGroup mixerGroupMic, mixerGroupMaster;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        //Microphone input
        if(useMicrophone == true)
        {
            if(Microphone.devices.Length > 0)
            {
                selectedDevice = Microphone.devices[0].ToString();
                audioSrc.outputAudioMixerGroup = mixerGroupMic;
                audioSrc.clip = Microphone.Start(selectedDevice, true, 2400, AudioSettings.outputSampleRate);

            }
            else
            {
                useMicrophone = false;
            }
        }
        else
        {
            audioSrc.outputAudioMixerGroup = mixerGroupMaster;
            audioSrc.clip = audioClip;
        }

        audioSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetSpectrumAudioSource()
    {
        audioSrc.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        //22050 / 512 = 43 hz per sample
        //20-60hz, 250-500hz, 500-2000hz, 2000-4000hz, 4000-6000hz, 6000-20000hz

        /*
         * 0 - 2 = 86hz
         * 1 - 4 = 172hz    87-258
         * 2 - 8 = 344hz    259-602
         * 3 - 16 = 688hz   603-1290
         * 4 - 32 = 1376hz  1291-2666
         * 5 - 64 = 2752hz  2667-5418
         * 6 - 128 = 5504   5419-10922 
         * 7 - 256 = 11008  10923-21930
         * = 510
         */

        int counter = 0;
        
        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for(int j = 0; j < sampleCount; j++)
            {
                average += samples[counter] * (counter + 1);
                counter++;
            }

            average /= counter;
            freqBands[i] = average * 10;
          
        }

    }

    void BandBuffer()
    {
        for(int g = 0; g < 8; g++)
        {
            if(freqBands[g] > bandBuffer[g])
            {
                bandBuffer[g] = freqBands[g];
                bufferDecrease[g] = 0.005f;
            }
            if (freqBands[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBands[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBands[i];

            }
            audioBand[i] = (freqBands[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }

        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }
}
