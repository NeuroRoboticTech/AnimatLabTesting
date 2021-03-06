﻿Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System
Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.WinControls
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse
Imports MouseButtons = System.Windows.Forms.MouseButtons
Imports AnimatTesting.Framework
Imports System.Xml

Namespace UITests
    Namespace ConversionTests
        Namespace NeuralTests

            <CodedUITest()>
            Public Class NeuralUIConversionUITest
                Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"
                '

#Region "Firing Rate Methods"

                <TestMethod()>
                Public Sub Test_FiringRate_BistableNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Il", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_BistableNeuron"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Set Vth to 12 mv
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vsth", "12 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vsth_12mv_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vsth", "10 m"})

                    'Set ih to 4na, Il to -1 na
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Ih", "4 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "-1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ih_4na_Il_-1na_")

                    'Set stim_3 to -5 nA
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_3", "CurrentOn", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim3_-5na_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_GatedSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF_1a", 0.01)
                    aryMaxErrors.Add("FF_1b", 0.01)
                    aryMaxErrors.Add("FF_2", 0.01)
                    aryMaxErrors.Add("FF_3", 0.01)
                    aryMaxErrors.Add("FF_4", 0.01)
                    aryMaxErrors.Add("FF_5", 0.01)
                    aryMaxErrors.Add("FF_6b", 0.01)
                    aryMaxErrors.Add("External_1a", 0.0000000001)
                    aryMaxErrors.Add("External_3", 0.0000000001)
                    aryMaxErrors.Add("Synaptic_2", 0.0000000001)
                    aryMaxErrors.Add("External_1b", 0.0000000001)
                    aryMaxErrors.Add("External_5", 0.0000000001)
                    aryMaxErrors.Add("Synaptic_4", 0.0000000001)
                    aryMaxErrors.Add("External_6b", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_GatedSynapse"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 1] 1 (5 nA))", "Weight", "0.5"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -1] 1 (5 nA))", "Weight", "-0.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_0_5_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 0.5] 1 (5 nA))", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -0.5] 1 (5 nA))", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 0.5] 1 (5 nA))", "Enabled", "True"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -0.5] 1 (5 nA))", "Enabled", "True"})

                    'swap settings.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 0.5] 1 (5 nA))", "Weight", "-1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -1] 1 (5 nA))", "GateInitiallyOn", "True"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -0.5] 1 (5 nA))", "Weight", "1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 1] 1 (5 nA))", "GateInitiallyOn", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapGates_")


                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -1] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\6 (1 nA)"})
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 1] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\6 (1 nA)"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (5 nA)", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\1 (5 nA)", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_9", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapLinkedSynapses_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -1] 6 (1 nA))", "Delete Link")
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 1] 6 (1 nA))", "Delete Link")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeletedGates_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_ModulatedSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF_1a", 0.01)
                    aryMaxErrors.Add("FF_1b", 0.01)
                    aryMaxErrors.Add("FF_2", 0.01)
                    aryMaxErrors.Add("FF_3", 0.01)
                    aryMaxErrors.Add("FF_4", 0.01)
                    aryMaxErrors.Add("FF_5", 0.01)
                    aryMaxErrors.Add("FF_6b", 0.01)
                    aryMaxErrors.Add("External_1a", 0.0000000001)
                    aryMaxErrors.Add("External_3", 0.0000000001)
                    aryMaxErrors.Add("Synaptic_2", 0.0000000001)
                    aryMaxErrors.Add("External_1b", 0.0000000001)
                    aryMaxErrors.Add("External_5", 0.0000000001)
                    aryMaxErrors.Add("Synaptic_4", 0.0000000001)
                    aryMaxErrors.Add("External_6b", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_ModulatedSynapse"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})


                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2] 1 (5 nA))", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2] 1 (5 nA))", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2] 1 (5 nA))", "Enabled", "True"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2] 1 (5 nA))", "Enabled", "True"})


                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2] 1 (5 nA))", "Gain", "2.5"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2] 1 (5 nA))", "Gain", "-2.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_2_5_")

                    'swap settings.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2.5] 1 (5 nA))", "Gain", "-2"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2.5] 1 (5 nA))", "Gain", "2"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapGates_")


                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -2] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\6 (1 nA)"})
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 2] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\6 (1 nA)"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (5 nA)", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\1 (5 nA)", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_9", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapLinkedSynapses_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -2] 6 (1 nA))", "Delete Link")
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 2] 6 (1 nA))", "Delete Link")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeletedSynapses_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_NormalNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Ie", 0.0000000001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_NormalNeuron"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Now decrease Cm from 3nf to 1nf
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Cm", "1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Cm1nf_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Cm", "3 n"})

                    'Now increase Gm from 100nS to 200nS
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gm", "200 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Gm200ns_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gm", "100 n"})

                    'Now increase Vth from 0 to 20mV
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vth", "20 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth20mv_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vth", "0 m"})

                    'Now increase FMin from 0 to 0.2 Hz
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Fmin", "0.2"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Fmin2_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Fmin", "0"})

                    'Now increase Gain from 15 to 20
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gain", "20"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Gain20_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gain", "15"})

                    'Now decrease Vrest from 0 to -70 mv and Vth
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vrest", "-70 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vth", "-70 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vrest-70mv_")

                    'Now increase accomodation from 0 to 1
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccommodation", "1"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom1_")

                    'Now decrease accomodation tc from 200 ms to 100 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccommodationTimeConstant", "100 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AccomTc100ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccommodation", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccommodationTimeConstant", "200 m"})

                    'Change Vnoise to 5 mv
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "VNoiseMax", "5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vnoise5mv_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_NormalSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF_1", 0.01)
                    aryMaxErrors.Add("FF_2", 0.01)
                    aryMaxErrors.Add("Vm_1", 0.0001)
                    aryMaxErrors.Add("Vm_2", 0.0001)
                    aryMaxErrors.Add("Ie_1", 0.0000000001)
                    aryMaxErrors.Add("Ii_1", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_NormalSynapse"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (-10 nAA)", "Weight", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_-5na_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (-5 nAA)", "Weight", "-15 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_-15na_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (-15 nAA)", "Weight", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_10na_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Vth", "-50 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Vrest", "-70 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "2_Vrest_-70mv_Vth_-50mv_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Ih", "-1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ih_-1na_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (10 nAA)", "Delete Link")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeletedSynapse_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_PacemakerNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Intrinsic", 0.0000000001)
                    aryMaxErrors.Add("External", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_PacemakerNeuron"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Now decrease Cm from 3nf to 1nf
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Ih", "2.5 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "-2.5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IhIl_2_5_")

                    'Now increase Gm from 100nS to 200nS
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Th", "0.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Th_0_5_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Th", "1"})

                    'Now increase Vth from 0 to 20mV
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Mtl", "-50"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Mtl_-50_")

                    'Now increase FMin from 0 to 0.2 Hz
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Btl", "1"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Btl_1_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Mtl", "-100"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Btl", "2"})

                    'Now increase Gain from 15 to 20
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vssm", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\C", "CurrentOn", "-2 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\D", "CurrentOn", "2 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vsm_0_5_")

                End Sub


                <TestMethod()>
                Public Sub Test_FiringRate_RandomNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Il", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_RandomNeuron"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    'RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Set Il to -5 na
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "-5 n"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Il_-5_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "0"})

                    'Change poly gains.
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "BurstLengthDistribution"}, 500)
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"C", "0.033 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"D", "0.01"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BLength_033_")

                    'ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B", "Gain"}, 500)
                    'ExecuteActiveDialogMethod("SetGainProperty", New Object() {"A", "0"})
                    'ExecuteActiveDialogMethod("SetGainProperty", New Object() {"B", "15 n"})
                    'ExecuteActiveDialogMethod("SetGainProperty", New Object() {"C", "0.001 n"})
                    'ExecuteActiveDialogMethod("SetGainProperty", New Object() {"D", "-1 n"})
                    'ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)




                    'I cannot do the tests below because the random number results are different from VS7 to VS10. It produces different double results.
                    'Now increase burst length duration to 0.023 
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "BurstLengthDistribution.C", "0.023"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BLength_023_")
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "BurstLengthDistribution.C", "0.013"})

                    ''Now increase Current size to 0.3 nA
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CurrentDistribution.C", "0.3 n"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CDist_03n_")
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CurrentDistribution.C", "0.1 n"})

                    ''Now increase inter-burst length duration to 0.022 
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InterburstLengthDistribution.C", "0.022"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IBLength_022_")
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InterburstLengthDistribution.C", "0.012"})

                End Sub


                <TestMethod()>
                Public Sub Test_FiringRate_TonicNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Il", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_TonicNeuron"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Ih", "5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ih_5na_")

                End Sub

#End Region

#Region "IGF Methods"

                <TestMethod()>
                Public Sub Test_IGF_ClassicalConditioning()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("3", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_ClassicalConditioning"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("CloneSynapseType", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "Excitatory Ach"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Excitatory Ach", "EquilibriumPotential", "0 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Excitatory Ach", "SynapticConductance", "2 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Excitatory Ach", "RelativeFacilitation", "1"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Excitatory Ach", "FacilitationDecay", "1 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Excitatory_Ach_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "EndTime", "140 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "StartTime", "130 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FoodAfterBell_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "StartTime", "120 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "EndTime", "130 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FoodAfterBellB_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "StartTime", "110 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "EndTime", "120 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FoodAfterBellC_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningIncrement", "0.2"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "LearnIncr_0_2_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "StimD_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\NeuralData\LineChart", "CollectEndTime", "2.5"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "StimC_")

                End Sub

                <TestMethod()>
                Public Sub Test_IGF_EndogenousBurster()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("Ica", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_EndogenousBurster"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "MaxCaConductance", "30 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MaxGaCond_30us_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "MaxCaConductance", "18 u"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.MidPoint", "-27 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Act_Mid_-27mv_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.MidPoint", "-35 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Act_Mid_-35mv_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.MidPoint", "-30 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.Slope", "0.09"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Act_Slope_0_09_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.Slope", "0.12"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.TimeConstant", "20 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Act_TC_20ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaActivation.TimeConstant", "8 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaDeactivation.MidPoint", "-72 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Deact_Mid_-72mv_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaDeactivation.MidPoint", "-90 m"})


                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaDeactivation.Slope", "-0.08"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Deact_Slope_-0_08_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaDeactivation.Slope", "-0.1"})


                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaDeactivation.TimeConstant", "4000 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Deact_Tc_4000ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CaDeactivation.TimeConstant", "2000 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "ApplyCd", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Cad_")

                End Sub


                <TestMethod()>
                Public Sub Test_IGF_IonChannels()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1Vm", 0.0001)
                    aryMaxErrors.Add("Delayed_Rect_K", 0.0000000001)
                    aryMaxErrors.Add("Fast_Na", 0.0000000001)
                    aryMaxErrors.Add("1It", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_IonChannels"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Minit", "0.35"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Nm", "2"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Nm_2_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Nm", "1"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Minf.D", "40 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Minf_D_40mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Minf.D", "44 m"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Tm.B", "5 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Tm_B_5m_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Tm.B", "4 m"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Hinit", "0.25"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Hinit_0_25_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Hinit", "0.65"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Hinf.D", "70 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Hinf_D_70m_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Hinf.D", "68 m"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Th.B", "15 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Th_B_15m_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "Th.B", "25 m"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "EquilibriumPotential", "65 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "EqPot_65m_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "EquilibriumPotential", "45 m"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Gmax", "50 n"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Gmax_50n_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "Gmax", "300 n"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "MPower", "5"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MPow_5_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "IonChannels"}, 500)
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Delayed Rect K", "MPower", "4"})
                    ExecuteActiveDialogMethod("SetListItemObjectProperty", New Object() {"Fast Na", "HPower", "2"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "HPow_2_")

                End Sub

                <TestMethod()>
                Public Sub Test_IGF_NonspikingChemicalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_NonspikingChemicalSynapses"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "EquilibriumPotential", "50 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_EqPot_50mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "EquilibriumPotential", "-10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "MaxSynapticConductance", "2 u"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_MaxCond_2uS_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "MaxSynapticConductance", "0.5 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "PreSynapticSaturationLevel", "-50 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_SatPot_-50mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "PreSynapticSaturationLevel", "-20 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "PreSynapticThreshold", "-35 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_ThreshPot_-35mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Hyperpolarising IPSP"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "HypIPSP_")

                End Sub


                <TestMethod()>
                Public Sub Test_IGF_ElectricalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_ElectricalSynapses"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "-30 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "CurrentOn", "-30 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim_-30nA_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "30 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "CurrentOn", "30 n"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse", "HighCoupling", "0.3 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse", "LowCoupling", "0.05 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse", "TurnOnThreshold", "0 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse", "TurnOnSaturate", "30 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Rectifying_Stim_30nA_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "-30 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "CurrentOn", "-30 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Rectifying_Stim_-30nA_")

                End Sub

                <TestMethod()>
                Public Sub Test_IGF_SpikingChemicalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("3", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_SpikingChemicalSynapses"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapticConductance", "2 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_2uS_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (2 uS)", "SynapticConductance", "1.5 u"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapticConductance", "2 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S2_2uS_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (2 uS)", "SynapticConductance", "0.5 u"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "ConductionDelay", "1.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "1_Delay_1_5_ms_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "ConductionDelay", "1.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "2_Delay_1_5_ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "ConductionDelay", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "ConductionDelay", "0"})

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "DecayRate", "5 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Decay_5ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "DecayRate", "30 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Decay_30ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "DecayRate", "15 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "DecayRate", "2 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S2_Decay_2ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "DecayRate", "50 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S2_Decay_50ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "EquilibriumPotential", "-30 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "DecayRate", "10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "EquilibriumPotential", "-90 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_EqPot_-30mv_S2_EqPot_-90mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "EquilibriumPotential", "-10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "FacilitationDecay", "20 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "EquilibriumPotential", "-70 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "FacilitationDecay", "300 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_FacilDecay_20ms_S2_FacilDecay_300ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "FacilitationDecay", "300 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "FacilitationDecay", "20 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_FacilDecay_300ms_S2_FacilDecay_20ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "FacilitationDecay", "100 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "RelativeFacilitation", "5"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "FacilitationDecay", "100 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "RelativeFacilitation", "5"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1S2_Facil_5_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "RelativeFacilitation", "0.9"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "RelativeFacilitation", "0.9"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1S2_Facil_0_9_")

                    'Voltage dependent tests
                    'Disabled Neuron 2, switch synapse 1 to NMDA type, REset stim to go from 50-60ms at 30 nA, reset chart to 0 to 200 ms.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Enabled", "False"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "StartTime", "50 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "EndTime", "60 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "30 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "N2_Disabled_S1_NMDA_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "SaturatePotential", "-40 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_SatPot_-40mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "MaxRelativeConductance", "15 u"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_MaxCond_15us_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "MaxRelativeConductance", "10 u"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\Neural Subsystem\3", "Stimulus_C") ', "Stimulus_1"
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "StartTime", "20 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "EndTime", "170 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim3_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim3_-5nA_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "ThresholdPotential", "-80 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Thresh_-80mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "VoltageDependent", "False"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_NoVoltageDep_")

                    ''Only used if commenting out upper portion to test hebbian.
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Enabled", "False"})
                    'ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    'ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type"})
                    'ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    'AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\Neural Subsystem\3", "Stimulus_C", "Stimulus_1")
                    ''Only used if commenting out upper portion to test hebbian.


                    'Hebbian tests
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccomodation", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3", "AHP_Conductance", "1.5 u"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "StartTime", "10 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "EndTime", "260 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "21 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Hebbian_")

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\Neural Subsystem\1", "Stimulus_D") ', "Stimulus_2"
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "StartTime", "110 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "EndTime", "160 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "CurrentOn", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_LowFreq_Increase_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "Enabled", "True"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "StartTime", "110 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "EndTime", "160 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_N2_10nA_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "30 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_N2_30nA_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "50 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_N2_50nA_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "10 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapticConductance", "0.6 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Cond_0_6uS_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.6 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "MaxAugmentedConductance", "1.5 u"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_MaxAugCond_1_5uS_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.6 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "MaxAugmentedConductance", "2 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningTimeWindow", "4 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_LearnWindow_4ms_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.6 uS)", "SynapticConductance", "0.4 u"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "MaxAugmentedConductance", "2 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningTimeWindow", "30 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningIncrement", "0.8"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_LearnIncr_0_8_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ForgettingTimeWindow", "10 m"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_ForgetWindow_10ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ConsolidationFactor", "1"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Consolidation_1_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ForgettingTimeWindow", "10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ConsolidationFactor", "20"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "AllowForgetting", "False"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_NoForgetting_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "Hebbian", "False"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_NotHebbian_")


                End Sub

                <TestMethod()>
                Public Sub Test_IGF_SpikingNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_SpikingNeuron"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ''Change AHP Eq Pot to -90 mv: AHP_EqPot_-90mv_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "AHPEquilibriumPotential", "-90 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHP_EqPot_-90mv_")

                    ''Change AHP Eq Pot to -70 mv, Vth=-55mv, Refractory Period=5ms: Vth_-55mv_Refr_5ms_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InitialThreshold", "-55 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "AHPEquilibriumPotential", "-70 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "RefractoryPeriod", "5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-55mv_Refr_5ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "RefractoryPeriod", "2 m"})

                    ''Change spike peak to 10 mv: SpikePeak_10mv_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "SpikePeak", "10 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SpikePeak_10mv_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "SpikePeak", "0 m"})

                    ''Apply TTX: TTX_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "ApplyTTX", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "TTX_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "ApplyTTX", "False"})

                    ''Change Vth to -50 mv: Vth_-50mv_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InitialThreshold", "-50 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50mv_")

                    'Vth=-50mv, Tc=50ms: Vth_-50_Tc_50ms_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TimeConstant", "50 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50_Tc_50ms_")

                    'Vth=-50mv, Tc=50ms, Size: 0.5: Vth_-50mv_Tc_5ms_Size_0_5_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TimeConstant", "5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeSize", "0.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50mv_Tc_5ms_Size_0_5_")

                    'Vth=-50mv, Tc=50ms, Size: 1.5: Vth_-50mv_Tc_5ms_Size_1_5
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeSize", "1.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50mv_Tc_5ms_Size_1_5_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeSize", "1"})

                    'Vth=-50mv, Tc=5ms, Accom=0,7: Tc_5ms_Accom_7_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccomodation", "0.7"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Tc_5ms_Accom_7_")

                    'Accom=0.7, Accom Tc=5ms: Accom_7_ATc_5ms_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccomodationTimeConstant", "5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom_7_ATc_5ms_")

                    'Accom=0.7, Accom Tc=15ms: Accom_7_ATc_15ms_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccomodationTimeConstant", "15 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom_7_ATc_15ms_")

                    'Accom=0.3, Accom Tc=10ms, AHP G=10uS: Accom_3_ATc_10ms_AHPG_10uS_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccomodation", "0.3"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccomodationTimeConstant", "10 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_Conductance", "10 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom_3_ATc_10ms_AHPG_10uS_")

                    'AHP G=10uS, AHP Tc=10ms: AHPG_10uS_AHPTc_10ms_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_TimeConstant", "10 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHPG_10uS_AHPTc_10ms_")

                    'AHP G=10uS, AHP Tc=10ms, Vrest=-55mv: AHPG_10uS_AHPTc_10ms_Vrest_-55mv_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RestingPotential", "-55 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHPG_10uS_AHPTc_10ms_Vrest_-55mv_")

                    'AHP G=1uS, AHP Tc=3ms, Vrest=-45mv: AHPG_1uS_AHPTc_3ms_Vrest_-45mv_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_Conductance", "1 u"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_TimeConstant", "3 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RestingPotential", "-45 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHPG_1uS_AHPTc_3ms_Vrest_-45mv_")

                    'Vrest=-70mv: Vrest_-70mv_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RestingPotential", "-70 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vrest_-70mv_")

                    'ITonic=10nA: Itonic_10na_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TonicStimulus", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Itonic_10na_")

                    'ITonic=0nA, INoise=5mV, Stim disabled: Itonic_10na_INoise_5mv_NoStim_
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TonicStimulus", "0 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TonicNoise", "5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Itonic_10na_INoise_5mv_NoStim_")

                End Sub

                <TestMethod()>
                Public Sub Test_IGF_LateralInhibition()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("7", 0.0001)
                    aryMaxErrors.Add("8", 0.0001)
                    aryMaxErrors.Add("9", 0.0001)
                    aryMaxErrors.Add("10", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_LateralInhibition"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                End Sub

                <TestMethod()>
                Public Sub Test_IGF_CompartmentalModel()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("A", 0.0001)
                    aryMaxErrors.Add("B", 0.0001)
                    aryMaxErrors.Add("V", 0.0001)
                    aryMaxErrors.Add("Soma", 0.0001)
                    aryMaxErrors.Add("DistalExcitation", 0.0001)
                    aryMaxErrors.Add("DistalInhibition", 0.0001)
                    aryMaxErrors.Add("ProximalInhibition", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_CompartmentalModel"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DistalInhib_")

                End Sub

#End Region

#Region "Mixed Methods"


                <TestMethod()>
                Public Sub Test_NetworkOscillators()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("3", 0.0001)
                    aryMaxErrors.Add("A", 0.01)
                    aryMaxErrors.Add("B", 0.01)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "NetworkOscillators"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\IGF_NeuralData")
                    'm_aryWindowsToOpen.Add("Tool Viewers\FF_NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                End Sub

                <TestMethod(), _
                DataSource("System.Data.OleDb", _
                           "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                           "PhysicsEngines", _
                           DataAccessMethod.Sequential), _
                DeploymentItem("TestCases.accdb")>
                Public Sub Test_Adapters()

                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Avm", 0.0001)
                    aryMaxErrors.Add("Bvm", 0.0001)
                    aryMaxErrors.Add("1vm", 0.0001)
                    aryMaxErrors.Add("2vm", 0.0001)
                    aryMaxErrors.Add("1FF", 0.01)
                    aryMaxErrors.Add("2FF", 0.01)
                    aryMaxErrors.Add("1Ia", 0.0000000001)
                    aryMaxErrors.Add("BIa", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "Adapters"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Change poly gains.
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"A", "0"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"B", "2 u"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"C", "0.1 u"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"D", "-10 n"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"A", "0"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"B", "15 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"C", "0.001 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"D", "-1 n"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PolyGain_")


                    'Change bell gains.
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SelectGainType", New Object() {"AnimatGUI.DataObjects.Gains.Bell"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Amplitude", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Width", "1000 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"XOffset", "-30 m"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"YOffset", "0"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SelectGainType", New Object() {"AnimatGUI.DataObjects.Gains.Bell"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Amplitude", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Width", "10 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"XOffset", "500 m"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"YOffset", "0"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BellGain_")


                    'Change Sigmoid gains.
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SelectGainType", New Object() {"AnimatGUI.DataObjects.Gains.Sigmoid"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Amplitude", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Steepness", "100 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"XOffset", "-40 m"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"YOffset", "-0.1 n"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SelectGainType", New Object() {"AnimatGUI.DataObjects.Gains.Sigmoid"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Amplitude", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Steepness", "25 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"XOffset", "0.5 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"YOffset", "0"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SigmoidGain_")

                    'Disabled adapters
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "Enabled", "True"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "SourceDataTypes", "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking.DataTypes.ExternalCurrent"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SelectGainType", New Object() {"AnimatGUI.DataObjects.Gains.Sigmoid"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Amplitude", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Steepness", "50 M"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"XOffset", "0 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"YOffset", "-5 n"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DataSourceType_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1\A", "Delete Link")
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B\F2", "Delete Link")

                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_1"})) Then
                        Throw New System.Exception("A_1 adapter was not deleted")
                    End If

                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2_B"})) Then
                        Throw New System.Exception("2_B adapter was not deleted")
                    End If
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteAdapters_")

                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem"}, 2000)
                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                      "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(316, 114), "OP")

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", "A", "", False, , True)

                    AssertErrorDialogShown("The off-page connector node 'OP' must be associated with another node before you can connect it with a link.", enumErrorTextType.Contains)


                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B"})

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", "", "", False)

                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2"})) Then
                        Throw New System.Exception("2 adapter was not added")
                    End If
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Name", "1_B"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1_B", "SourceDataTypes", "FiringRateGUI.DataObjects.Behavior.Neurons.Normal.DataTypes.FiringFrequency"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1_B", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SelectGainType", New Object() {"AnimatGUI.DataObjects.Gains.Sigmoid"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Amplitude", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"Steepness", "10 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"XOffset", "0.5 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"YOffset", "0"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F2", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1", "S_F2_F1", "Normal Synapse", False)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1\F2 (S_F2_F1)", "Weight", "100 n"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Adapter1B_")

                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1_B", "Name", "1_A"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_A", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Adapter1A_")


                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1\F2 (100 nA)", "Delete Link")
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1_A", "Delete Node")
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1"})
                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F2", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", "S_F2_F1", "Normal Synapse", False)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F1\F2 (S_F2_F1)", "Weight", "100 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "F2ToOPtoF1_")

                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F2"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F2\F2 (100 nA)", "Weight", "5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "F2ToOPtoF2_")

                    'Change linked item to neuron in different module. Verify synapse deleted.
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\OP", _
                                                                  "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A"})
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F2\F2 (100 nA)"})) Then
                        Throw New System.Exception("2 adapter was not deleted")
                    End If
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "InvalidLinkNeuron_")

                End Sub

                <TestMethod()>
                Public Sub Test_CopyPasteCut()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("S1I1Vm", 0.0001)
                    aryMaxErrors.Add("S1I2Vm", 0.0001)
                    aryMaxErrors.Add("S1F1Vm", 0.0001)
                    aryMaxErrors.Add("S1F2Vm", 0.0001)
                    aryMaxErrors.Add("S1F1FF", 0.01)
                    aryMaxErrors.Add("S1F2FF", 0.01)
                    aryMaxErrors.Add("S2F4", 0.01)
                    aryMaxErrors.Add("S2F1", 0.01)
                    aryMaxErrors.Add("S3F1", 0.01)
                    aryMaxErrors.Add("S1F1Ia", 0.0000000001)
                    aryMaxErrors.Add("S1I2Ia", 0.0000000001)
                    aryMaxErrors.Add("S2I2", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "TestCopyPasteCut"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", "Name", "S1"})
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1"}, 2000)
                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1", _
                                      "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(316, 114), "S1OP1")
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", _
                                                                  "Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2"})
                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", _
                                       "Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "", "", False)

                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\2"})) Then
                        Throw New System.Exception("2 adapter was not added")
                    End If
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\2", "Name", "S1F1_S1I2"})

                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\2"})) Then
                        Throw New System.Exception("S1F1_S1I2 adapter was not renamed correctly. 2 node still found")
                    End If
                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2"})) Then
                        Throw New System.Exception("S1F1_S1I2 adapter was not renamed correctly.")
                    End If
                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2\S1F1"})) Then
                        Throw New System.Exception("S1F1 link for the S1F1_S1I2 adapter was not found.")
                    End If
                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2\S1F1_S1I2"})) Then
                        Throw New System.Exception("S1F1_S1I2 link for the S1I2 node was not found.")
                    End If

                    'Now change the name of the S1F2 neuron to S1F4 and verify that the names changed correctly in the treeview.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F2", "Name", "S1F4"})

                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F2"})) Then
                        Throw New System.Exception("S1F4 node was not renamed correctly. S1F2 node still found")
                    End If
                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4"})) Then
                        Throw New System.Exception("S1F4 node was not renamed correctly.")
                    End If
                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1\S1F4 (100 nA)"})) Then
                        Throw New System.Exception("S1F4->S1F1 link was not renamed correctly.")
                    End If

                    'Check the in/out links of the nodes
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "InLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "OutLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "InLinks.Count"), Integer), 1, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "OutLinks.Count"), Integer), 0, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "InLinks.Count"), Integer), 1, "S1OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "OutLinks.Count"), Integer), 0, "S1OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "InLinks.Count"), Integer), 1, "S1F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "OutLinks.Count"), Integer), 1, "S1F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "InLinks.Count"), Integer), 0, "S1F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "OutLinks.Count"), Integer), 1, "S1F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "InLinks.Count"), Integer), 1, "S1F1_S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "OutLinks.Count"), Integer), 1, "S1F1_S1I2 inlink count")

                    'Change poly gains.
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"A", "0 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"B", "0 "})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"C", "10 n"})
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"D", "0 "})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterOffpageAdded_")

                    '1. copy S1F1, S1F4 and liks to new subsystem. Veryify that new S1F4 gets stim different than S2F4.
                    TestCCP_CopyF1F4(aryMaxErrors)

                    '2. copy S1I2, S1OP1, S1F1, S1F4, S1F1_S1I2 and liks to new subsystem. Veryify that new S1I2 gets stim from new S1F1/S1F2 by disabling adapters. Add some params to data file.
                    TestCCP_CopyF1F4OPI2(aryMaxErrors)

                    '3. Copy S2 subsystem as S3 and verify everything connect correctly.
                    TestCCP_CopyS2ToS3(aryMaxErrors)

                    '4. Cut B, S1OP1, S1F1, S1F2, S1F1_S1I2 and links to new subsystem. Verify old system still works and charts correctly. Verify charts and stims deleted and then readd.
                    TestCCP_CutAllToS2(aryMaxErrors)

                End Sub

                Protected Sub TestCCP_CopyF1F4(ByVal aryMaxErrors As Hashtable)

                    'Add subsystem.
                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1", _
                                      "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(316, 30), "S2")

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", False})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1\S1F4 (100 nA)", True})
                    ClickMenuItem("CopyToolStripMenuItem", True)
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2"}, 2000)
                    ClickMenuItem("PasteInPlaceToolStripMenuItem", True)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F1", "Name", "S2F1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F4", "Name", "S2F4"})

                    'Verify that the pasted nodes have the same locations on the page as the copied ones since we did a paste in place.
                    Dim ptF1ALoc As PointF = DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "Location"), PointF)
                    Dim ptF4ALoc As PointF = DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "Location"), PointF)
                    Dim ptF1BLoc As PointF = DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1", "Location"), PointF)
                    Dim ptF4BLoc As PointF = DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F4", "Location"), PointF)

                    If ptF1ALoc <> ptF1BLoc OrElse ptF4ALoc <> ptF4BLoc Then
                        Throw New System.Exception("Locations did not match after paste in place.")
                    End If

                    'Check the in/out links of the nodes
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "InLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "OutLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "InLinks.Count"), Integer), 1, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "OutLinks.Count"), Integer), 0, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "InLinks.Count"), Integer), 1, "S1OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "OutLinks.Count"), Integer), 0, "S1OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "InLinks.Count"), Integer), 1, "S1F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "OutLinks.Count"), Integer), 1, "S1F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "InLinks.Count"), Integer), 0, "S1F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "OutLinks.Count"), Integer), 1, "S1F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "InLinks.Count"), Integer), 1, "S1F1_S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "OutLinks.Count"), Integer), 1, "S1F1_S1I2 inlink count")

                    'Add these neurons to the chart.
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\NeuralData"}, 2000)
                    ClickToolbarItem("AddAxisToolStripButton", True)
                    AddItemToChart("Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S2F1")
                    AddItemToChart("Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S2F4")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PasteF1F4_")

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S2\S2F4", "Stim_S2F4")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S2F4", "EndTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S2F4", "CurrentOn", "1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "StimS2F4_")

                    'Delete pasted items and make sure they are gone.
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1", "Delete Node")
                    Threading.Thread.Sleep(1000)
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1"})) Then
                        Throw New System.Exception("S2F1 node was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1\S2F4 (100 nA)"})) Then
                        Throw New System.Exception("S2F4->S2F1 link was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\NeuralData\LineChart\Y Axis 6\S2F1"})) Then
                        Throw New System.Exception("S2F1 chart node was not removed correctly.")
                    End If

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F4", "Delete Node")
                    Threading.Thread.Sleep(200)
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F4"})) Then
                        Throw New System.Exception("S2F4 node was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\NeuralData\LineChart\Y Axis 6\S2F4"})) Then
                        Throw New System.Exception("S2F4 chart node was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Stimuli\Stim_S2F4"})) Then
                        Throw New System.Exception("Stim_S2F4 stimulus was not removed correctly.")
                    End If
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterOffpageAdded_")

                End Sub

                Protected Sub TestCCP_CopyF1F4OPI2(ByVal aryMaxErrors As Hashtable)

                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1"}, 2000)
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", False})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1\S1F4 (100 nA)", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2\S1F1_S1I2", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2\S1F1", True})
                    ClickMenuItem("CopyToolStripMenuItem", True)
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2"}, 2000)
                    ClickMenuItem("PasteInPlaceToolStripMenuItem", True)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F1", "Name", "S2F1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F4", "Name", "S2F4"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1I2", "Name", "S2OP1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F1_S1I2", "Name", "S2F1_S1I2"})

                    'Check the in/out links of the nodes
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "InLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "OutLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "InLinks.Count"), Integer), 2, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "OutLinks.Count"), Integer), 0, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "InLinks.Count"), Integer), 1, "S1OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", "OutLinks.Count"), Integer), 0, "S1OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "InLinks.Count"), Integer), 1, "S1F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", "OutLinks.Count"), Integer), 1, "S1F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "InLinks.Count"), Integer), 0, "S1F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", "OutLinks.Count"), Integer), 1, "S1F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "InLinks.Count"), Integer), 1, "S1F1_S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", "OutLinks.Count"), Integer), 1, "S1F1_S1I2 inlink count")

                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2OP1", "InLinks.Count"), Integer), 1, "S2OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2OP1", "OutLinks.Count"), Integer), 0, "S2OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1", "InLinks.Count"), Integer), 1, "S2F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1", "OutLinks.Count"), Integer), 1, "S2F1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F4", "InLinks.Count"), Integer), 0, "S2F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F4", "OutLinks.Count"), Integer), 1, "S2F4 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1_S1I2", "InLinks.Count"), Integer), 1, "S2F1_S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1_S1I2", "OutLinks.Count"), Integer), 1, "S2F1_S1I2 inlink count")

                    'Add these neurons to the chart.
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\NeuralData"}, 2000)
                    ClickToolbarItem("AddAxisToolStripButton", True)
                    AddItemToChart("Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S2F1")

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S2\S2F4", "Stim_S2F4")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S2F4", "EndTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S2F4", "CurrentOn", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "StimS2F4_2_")

                    'Switch S2OP1 to point to S1I1 instead.
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2OP1", _
                                               "Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1"})

                    'Check the in/out links of the nodes
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "InLinks.Count"), Integer), 1, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", "OutLinks.Count"), Integer), 0, "S1I1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "InLinks.Count"), Integer), 1, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", "OutLinks.Count"), Integer), 0, "S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2OP1", "InLinks.Count"), Integer), 1, "S2OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2OP1", "OutLinks.Count"), Integer), 0, "S2OP1 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1_S1I2", "InLinks.Count"), Integer), 1, "S2F1_S1I2 inlink count")
                    AssertMatch(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1_S1I2", "OutLinks.Count"), Integer), 1, "S2F1_S1I2 inlink count")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwitchS2OP1ToS1I1_")

                End Sub

                Protected Sub TestCCP_CopyS2ToS3(ByVal aryMaxErrors As Hashtable)

                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1"}, 2000)
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2", False})
                    ClickMenuItem("CopyToolStripMenuItem", True)
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2"}, 2000)
                    ClickMenuItem("PasteInPlaceToolStripMenuItem", True)
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2", "Name", "S3"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3\S2F1", "Name", "S3F1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3\S2F4", "Name", "S3F4"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3\S1I1", "Name", "S3OP1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3\S2F1_S1I2", "Name", "S3F1_S1I2"})

                    'Add these neurons to the chart.
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\NeuralData"}, 2000)
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\NeuralData\LineChart\Y Axis 6", False})
                    AddItemToChart("Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S3\Nodes\S3F1")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CopyS3_")

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S2\S3\S3F4", "Stim_S3F4")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S3F4", "StartTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S3F4", "EndTime", "0.2 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S3F4", "CurrentOn", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "StimS3F4_")

                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3"}, 2000)
                    'Switch S3OP1 to point to S2F1 instead.
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3\S3OP1", _
                                               "Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S2F1"})

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S3\S3F1_S1I2", "Gain"}, 500)
                    ExecuteActiveDialogMethod("SetGainProperty", New Object() {"C", "20 n"})
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S3_S3OP1_To_S2F1_")

                    'Delete S2 and verify output.
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1"}, 2000)
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2", "Delete Node")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteS2_")

                End Sub

                Protected Sub TestCCP_CutAllToS2(ByVal aryMaxErrors As Hashtable)

                    'Add subsystem.
                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\S1", _
                                      "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(316, 30), "S2")

                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1"}, 2000)
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1", False})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F4", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1\S1F4 (100 nA)", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1OP1", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2\S1F1_S1I2", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2\S1F1", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I1", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1I2", True})
                    DeleteSelectedParts("Delete Group", True)
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2"}, 2000)
                    ClickMenuItem("PasteInPlaceToolStripMenuItem", True)

                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1I2", "Name", "S1I2_Temp"})
                    'This is a somewhat dangerous method because it depends on the order of the items in the treeview. Do not see another simple way of doing it though
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1I2", "Name", "S1OP1"})
                    'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1I2_Temp", "Name", "S1I2"})

                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\NeuralData\LineChart\Y Axis 1\S1I1Vm"})) Then
                        Throw New System.Exception("S1I1Vm chart node was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\NeuralData\LineChart\Y Axis 2\S1F1Vm"})) Then
                        Throw New System.Exception("S1F1Vm chart node was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\NeuralData\LineChart\Y Axis 6\S2F1"})) Then
                        Throw New System.Exception("S2F1 chart node was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Stimuli\Stim_S1F2"})) Then
                        Throw New System.Exception("Stim_S1F2 stimulus was not removed correctly.")
                    End If
                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Stimuli\Stim_S1I1"})) Then
                        Throw New System.Exception("Stim_S1I1 stimulus was not removed correctly.")
                    End If

                    'Add these neurons to the chart.
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\NeuralData"}, 2000)
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 1", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1I1", "S1I1", "S1I1Vm")
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 1", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1I2", "S1I2", "S1I2Vm")

                    ClickToolbarItem("AddAxisToolStripButton", True)
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 2", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1F1", "S1F1", "S1F1Vm", "FiringRateGUI.DataObjects.Behavior.Neurons.Normal.DataTypes.MembraneVoltage")
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 2", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1F4", "S1F4", "S1F2Vm", "FiringRateGUI.DataObjects.Behavior.Neurons.Normal.DataTypes.MembraneVoltage")

                    ClickToolbarItem("AddAxisToolStripButton", True)
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 3", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1F1", "S1F1", "S1F1Ia", "FiringRateGUI.DataObjects.Behavior.Neurons.Normal.DataTypes.AdapterCurrent")
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 3", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1I2", "S1I2", "S1I2Ia", "IntegrateFireGUI.DataObjects.Behavior.Neurons.Spiking.DataTypes.AdapterCurrent")

                    ClickToolbarItem("AddAxisToolStripButton", True)
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 4", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1F1", "S1F1", "S1F1FF")
                    AddItemToChart("Tool Viewers\NeuralData\LineChart\Y Axis 4", "Simulation\Organism_1\Behavioral System\S1\Nodes\S2\Nodes\S1F4", "S1F4", "S1F2FF")

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S2\S1F4", "Stim_S1F4")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F4", "StartTime", "0 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F4", "EndTime", "1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F4", "ValueType", "Equation"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F4", "Equation", "20*t"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterOffpageAdded_")

                    'Copy a bunch of the nodes back without links. Ensure that the adapter does not get copied, and that none of the new nodes interact with the old ones.
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2"}, 2000)
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F1", False})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F4", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1F1_S1I2", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1I1", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S2\S1I2", True})
                    ClickMenuItem("CopyToolStripMenuItem", True)
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1"}, 2000)
                    ClickMenuItem("PasteInPlaceToolStripMenuItem", True)

                    If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\S1\S1F1_S1I2"})) Then
                        Throw New System.Exception("S1F1_S1I2 node was incorrectly copied over to S1.")
                    End If

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S1F1", "Stim_S1F1")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F1", "StartTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F1", "EndTime", "0.2 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1F1", "CurrentOn", "10 n"})
                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S1F4", "StimB_S1F4")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\StimB_S1F4", "StartTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\StimB_S1F4", "EndTime", "0.2 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\StimB_S1F4", "CurrentOn", "10 n"})
                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S1I1", "Stim_S1I1")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1I1", "StartTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1I1", "EndTime", "0.2 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1I1", "CurrentOn", "10 n"})
                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\S1\S1I2", "Stim_S1I2")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1I2", "StartTime", "0.1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1I2", "EndTime", "0.2 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_S1I2", "CurrentOn", "10 n"})

                    'This should have no effect on the old neurons.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterOffpageAdded_")

                End Sub

                <TestMethod()>
                Public Sub Test_CurrentStimuli()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Iext", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0000000001)

                    m_strProjectName = "CurrentStimuli"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Constant_Stim", "ValueType", "Constant"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "TonicCurrent_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Constant_Stim", "CurrentOn", "1 n"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "TonicCurrent_1na_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Constant_Stim", "ValueType", "Equation"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Constant_Stim", "Equation", "0.00000001*sin(5*t)"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Equation_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Constant_Stim", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Repetitive_Stim", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Repetitive_On_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Repetitive_Stim", "CurrentOff", "-5 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Repetitive_Stim", "CurrentOn", "5 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Repetitive_Stim", "CycleOnDuration", "100 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Repetitive_Stim", "CycleOffDuration", "200 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Repetitive_Set_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Repetitive_Stim", "Enabled", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Burst_On_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "CurrentOff", "-5 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "CurrentOn", "5 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "CurrentBurstOff", "-1 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "CycleOnDuration", "20 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "CycleOffDuration", "20 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "BurstOnDuration", "200 m"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Burst_Stim", "BurstOffDuration", "200 m"})

                    Dim aryIgnoreRows As New ArrayList
                    aryIgnoreRows.Add(New Point(451, 451))

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Burst_Set_", -1, aryIgnoreRows)

                End Sub


                <TestMethod()>
                Public Sub Test_IGF_SynapseTypes()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("A", 0.00001)
                    aryMaxErrors.Add("B", 0.00001)
                    aryMaxErrors.Add("C", 0.00001)
                    aryMaxErrors.Add("D", 0.00001)
                    aryMaxErrors.Add("E", 0.00001)
                    aryMaxErrors.Add("F", 0.00001)
                    aryMaxErrors.Add("default", 0.00001)

                    m_strProjectName = "IGF_SynapseTypes"
                    m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"MaxSynapticConductance", "1 u"})

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"HighCoupling", "0.5 u"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"LowCoupling", "0.5 u"})

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"DecayRate", "20 m"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Change1_")

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh"})
                    ExecuteActiveDialogMethod("Automation_CloneSynapseType", New Object() {"Nicotinic ACh 2"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"EquilibriumPotential", "-40 m"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"DecayRate", "5 m"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CloneSpiking_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type"})
                    ExecuteActiveDialogMethod("Automation_CloneSynapseType", New Object() {"Nicotinic ACh type 2"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"EquilibriumPotential", "0 m"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CloneNonSpiking_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse"})
                    ExecuteActiveDialogMethod("Automation_CloneSynapseType", New Object() {"Non-Rectifying Synapse 2"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"HighCoupling", "0.1 u"})
                    ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"LowCoupling", "0.1 u"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CloneElectrical_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Spiking Chemical Synapses"})
                    ExecuteActiveDialogMethod("Automation_AddSynapseType", New Object() {"New Synapse 1"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "NewSpiking_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses"})
                    ExecuteActiveDialogMethod("Automation_AddSynapseType", New Object() {"New Synapse 2"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "NewNonSpiking_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses"})
                    ExecuteActiveDialogMethod("Automation_AddSynapseType", New Object() {"Non-Rectifying Synapse 3"})

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "NewElectrical_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A (0.5 uS)", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Spiking Chemical Synapses\New Synapse 1"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    ExecuteIndirectActiveDialogMethod("SelectItemInComboBox", New Object() {"NMDA type"}, 2000, , True)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type"}, 2000)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteSpiking_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\New Synapse 2"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    ExecuteIndirectActiveDialogMethod("SelectItemInComboBox", New Object() {"Depolarising IPSP"}, 2000, , True)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Depolarising IPSP"}, 2000)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteNonSpiking_")


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse 3"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    ExecuteIndirectActiveDialogMethod("SelectItemInComboBox", New Object() {"Rectifying Synapse"}, 2000, , True)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Rectifying Synapse"}, 2000)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteElectrical_")

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse 2"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    ExecuteIndirectActiveDialogMethod("SelectItemInComboBox", New Object() {"Non-Rectifying Synapse"}, 2000, , True)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse"}, 2000)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    ExecuteIndirectActiveDialogMethod("SelectItemInComboBox", New Object() {"Rectifying Synapse"}, 2000, , True)
                    ExecuteIndirectActiveDialogMethod("ClickCancelButton", Nothing)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Non-Rectifying Synapse"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Rectifying Synapse"}, 2000)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)


                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Rectifying Synapse"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    AssertErrorDialogShown("You cannot delete the last synapse type because there are still synapses that use it and we have nothing to replace it with.", enumErrorTextType.Contains)

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\F\E", "Delete Link")

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", False})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C", "SynapseType"}, 500)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Electrical Synapses\Rectifying Synapse"})
                    ExecuteIndirectActiveDialogMethod("Automation_DeleteSynapseType", Nothing, 2000, , True)

                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, , True, True)

                    AssertErrorDialogShown("You must select a synapse type or hit cancel.", enumErrorTextType.Contains)

                    ExecuteActiveDialogMethod("Automation_SelectSynapseType", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Depolarising IPSP"}, 2000)
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                End Sub

#End Region

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    'This test compares data to that generated from the old version. We never re-generate the data in V2, so this should always be false 
                    'regardless of the setting in app.config.
                    m_bGenerateTempates = False
                    SetStructureNames("1", False)

                End Sub

                <TestCleanup()> Public Overrides Sub MyTestCleanup()
                    MyBase.MyTestCleanup()
                End Sub

                Protected Overrides Sub SetWindowsToOpen()
                End Sub

#End Region

#End Region

            End Class

        End Namespace
    End Namespace
End Namespace

