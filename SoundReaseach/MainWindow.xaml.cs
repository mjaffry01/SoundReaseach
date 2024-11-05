// MainWindow.xaml.cs
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SoundReaseach;


namespace SoundResearch
{
    public partial class MainWindow : Window
    {
        private IWavePlayer waveOut;
        private SignalGenerator signalGenerator;
        private RecordVoice recorder;
        private bool isRecording = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAudio();
            recorder = new RecordVoice();

        }

        private void InitializeAudio()
        {
            waveOut = new WaveOutEvent();
            signalGenerator = new SignalGenerator()
            {
                Gain = (float)AmplitudeSlider.Value,
                Frequency = FrequencySlider.Value,
                Type = SignalGeneratorType.Sin
            };
            waveOut.Init(signalGenerator);
            waveOut.Play();
        }

        // 1. Button Click Event Handlers
       
        
           private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                recorder.StartRecording();
                isRecording = true;
                RecordButton.Content = "Stop Recording";
                MessageBox.Show("Recording started.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                recorder.StopRecording();
                isRecording = false;
                RecordButton.Content = "Record";
                MessageBox.Show("Recording stopped.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                // Access the recorded data
                byte[] recordedData = recorder.RecordedBytes;

                // You can now process the recordedData as needed
                // For example, play it back or save to a file
                PlayRecordedData(recordedData);
            }
        }

        private void PlayRecordedData(byte[] data)
        {
            try
            {
                using (var ms = new MemoryStream(data))
                using (var rdr = new WaveFileReader(ms))
                using (var playback = new WaveOutEvent())
                {
                    playback.Init(rdr);
                    playback.Play();
                    while (playback.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during playback: {ex.Message}");
            }
        }






        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            waveOut.Pause();
            MessageBox.Show("Playback paused.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            waveOut.Play();
            MessageBox.Show("Playback started.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            waveOut.Stop();
            MessageBox.Show("Playback stopped.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement saving logic here
            MessageBox.Show("Save functionality not implemented.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 2. Slider and Control Event Handlers
        private void FrequencySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (signalGenerator != null)
            {
                signalGenerator.Frequency = e.NewValue;
            }
        }

        private void AmplitudeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (signalGenerator != null)
            {
                signalGenerator.Gain = (float)e.NewValue;
            }
        }

        private void WaveformComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (signalGenerator != null && WaveformComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string waveform = selectedItem.Content.ToString();
                switch (waveform)
                {
                    case "Sine":
                        signalGenerator.Type = SignalGeneratorType.Sin;
                        break;
                    case "Square":
                        signalGenerator.Type = SignalGeneratorType.Square;
                        break;
                    case "Sawtooth":
                        signalGenerator.Type = SignalGeneratorType.SawTooth;
                        break;
                    case "Triangle":
                        signalGenerator.Type = SignalGeneratorType.Triangle;
                        break;
                    default:
                        signalGenerator.Type = SignalGeneratorType.Sin;
                        break;
                }
            }
        }

        private void AttackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double attackTime = e.NewValue;
            // Update the attack time in the envelope generator
            // Example: soundEngine.SetAttackTime(attackTime);
        }

        private void DecaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double decayTime = e.NewValue;
            // Update the decay time in the envelope generator
            // Example: soundEngine.SetDecayTime(decayTime);
        }

        private void SustainSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double sustainLevel = e.NewValue;
            // Update the sustain level in the envelope generator
            // Example: soundEngine.SetSustainLevel(sustainLevel);
        }

        private void ReleaseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double releaseTime = e.NewValue;
            // Update the release time in the envelope generator
            // Example: soundEngine.SetReleaseTime(releaseTime);
        }

        // 3. Filter Controls Event Handlers
        private void FilterTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (signalGenerator != null && FilterTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string filterType = selectedItem.Content.ToString();
                // Implement filter type change logic
                // Example: soundEngine.SetFilterType(filterType);
            }
        }

        private void CutoffSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double cutoffFrequency = e.NewValue;
            // Implement cutoff frequency adjustment
            // Example: soundEngine.SetCutoffFrequency(cutoffFrequency);
        }

        private void ResonanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double resonance = e.NewValue;
            // Implement resonance adjustment
            // Example: soundEngine.SetResonance(resonance);
        }

        // 4. Modulation Controls Event Handlers
        private void EnableFMCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Enable frequency modulation
            // Example: soundEngine.EnableFrequencyModulation(true);
            MessageBox.Show("Frequency Modulation Enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableFMCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable frequency modulation
            // Example: soundEngine.EnableFrequencyModulation(false);
            MessageBox.Show("Frequency Modulation Disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void FMDepthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double fmDepth = e.NewValue;
            // Implement FM depth adjustment
            // Example: soundEngine.SetFMDepth(fmDepth);
        }

        private void FMRateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double fmRate = e.NewValue;
            // Implement FM rate adjustment
            // Example: soundEngine.SetFMRate(fmRate);
        }

        // 5. Pan Control Event Handler
        private void PanSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double panValue = e.NewValue;
            // Implement pan adjustment
            // Example: soundEngine.SetPan(panValue);
        }

        // 6. Effects Controls Event Handlers
        private void EnableReverbCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Enable reverb effect
            MessageBox.Show("Reverb Enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableReverbCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable reverb effect
            MessageBox.Show("Reverb Disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReverbLevelSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double reverbLevel = e.NewValue;
            // Implement reverb level adjustment
            // Example: soundEngine.SetReverbLevel(reverbLevel);
        }

        private void EnableEchoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Enable echo effect
            MessageBox.Show("Echo Enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableEchoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable echo effect
            MessageBox.Show("Echo Disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EchoDelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double echoDelay = e.NewValue;
            // Implement echo delay adjustment
            // Example: soundEngine.SetEchoDelay(echoDelay);
        }

        // 7. Equalizer Controls Event Handlers
        private void LowEQSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double lowEQGain = e.NewValue;
            // Implement low-frequency EQ gain adjustment
            // Example: soundEngine.SetLowEQGain(lowEQGain);
        }

        private void MidEQSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double midEQGain = e.NewValue;
            // Implement mid-frequency EQ gain adjustment
            // Example: soundEngine.SetMidEQGain(midEQGain);
        }

        private void HighEQSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double highEQGain = e.NewValue;
            // Implement high-frequency EQ gain adjustment
            // Example: soundEngine.SetHighEQGain(highEQGain);
        }

        // 8. Playback Speed and Direction Controls Event Handlers
        private void PlaybackSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double playbackSpeed = e.NewValue;
            // Implement playback speed adjustment
            // Example: soundEngine.SetPlaybackSpeed(playbackSpeed);
        }

        private void ReversePlaybackButton_Checked(object sender, RoutedEventArgs e)
        {
            // Enable reverse playback
            MessageBox.Show("Reverse Playback Enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReversePlaybackButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable reverse playback
            MessageBox.Show("Reverse Playback Disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 9. Noise Reduction Controls Event Handlers
        private void EnableNoiseGateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Enable noise gate
            MessageBox.Show("Noise Gate Enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableNoiseGateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable noise gate
            MessageBox.Show("Noise Gate Disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NoiseGateThresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double threshold = e.NewValue;
            // Implement noise gate threshold adjustment
            // Example: soundEngine.SetNoiseGateThreshold(threshold);
        }

        // 10. Sampling Rate and Bit Depth Controls Event Handlers
        private void SamplingRateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SamplingRateComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string samplingRate = selectedItem.Content.ToString();
                // Implement sampling rate update
                // Example: soundEngine.SetSamplingRate(samplingRate);
            }
        }

        private void BitDepthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BitDepthComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string bitDepth = selectedItem.Content.ToString();
                // Implement bit depth update
                // Example: soundEngine.SetBitDepth(bitDepth);
            }
        }

        // 11. Sound Level Display
        // Implement logic to update the SoundLevelProgressBar based on audio input
        // Example: soundEngine.OnSoundLevelChanged += (level) => Dispatcher.Invoke(() => SoundLevelProgressBar.Value = level);

        // 12. Spatial Position Controls Event Handlers
        private void SpatialXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double xPosition = e.NewValue;
            // Implement spatial X position adjustment
            // Example: soundEngine.SetSpatialXPosition(xPosition);
        }

        private void SpatialYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double yPosition = e.NewValue;
            // Implement spatial Y position adjustment
            // Example: soundEngine.SetSpatialYPosition(yPosition);
        }

        // 13. Input Gain Controls Event Handlers
        private void InputGainSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double inputGain = e.NewValue;
            // Implement input gain adjustment
            // Example: soundEngine.SetInputGain(inputGain);
        }

        private void EnableMonitoringCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Enable monitoring of input signal
            MessageBox.Show("Input Monitoring Enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnableMonitoringCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable monitoring of input signal
            MessageBox.Show("Input Monitoring Disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 14. Loop Points Controls Event Handlers
        private void SetLoopStartButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement loop start point setting
            MessageBox.Show("Loop Start Point Set.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SetLoopEndButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement loop end point setting
            MessageBox.Show("Loop End Point Set.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 15. Harmonic Emphasis Control Event Handler
        private void HarmonicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double harmonicEmphasis = e.NewValue;
            // Implement harmonic emphasis adjustment
            // Example: soundEngine.SetHarmonicEmphasis(harmonicEmphasis);
        }

        // 16. Phase Adjustment Control Event Handler
        private void PhaseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double phase = e.NewValue;
            // Implement phase adjustment
            // Example: soundEngine.SetPhase(phase);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
