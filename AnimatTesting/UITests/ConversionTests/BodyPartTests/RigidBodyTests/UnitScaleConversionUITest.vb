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
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class UnitScaleConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"

                    Protected m_iSubSteps As Integer = 1

#End Region

#Region "Properties"

#End Region

#Region "Methods"
                    '

                    <TestMethod(), _
                      DataSource("System.Data.OleDb", _
                                 "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                 "PhysicsEngines", _
                                 DataAccessMethod.Sequential), _
                      DeploymentItem("TestCases.accdb")>
                    Public Sub Test_UnitScale_Kg_M()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Rotation", 0.05)
                        aryMaxErrors.Add("PendulumX", 0.05)
                        aryMaxErrors.Add("PendulumY", 0.05)
                        aryMaxErrors.Add("PendulumZ", 0.05)
                        aryMaxErrors.Add("default", 0.05)

                        m_strProjectName = "UnitScale_Kg_M"
                        m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Y", "-7 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge", "WorldPosition.Y", "0 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "L_7m_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MotorStim", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "F_10N_")

                    End Sub


                    <TestMethod(), _
                      DataSource("System.Data.OleDb", _
                                 "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                 "PhysicsEngines", _
                                 DataAccessMethod.Sequential), _
                      DeploymentItem("TestCases.accdb")>
                    Public Sub Test_UnitScale_g_dm()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        m_iSubSteps = 10

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Rotation", 0.05)
                        aryMaxErrors.Add("PendulumX", 0.05)
                        aryMaxErrors.Add("PendulumY", 0.05)
                        aryMaxErrors.Add("PendulumZ", 0.05)
                        aryMaxErrors.Add("default", 0.05)

                        m_strProjectName = "UnitScale_g_dm"
                        m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Y", "-70 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge", "WorldPosition.Y", "0 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "L_70cm_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MotorStim", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "F_5N_")

                    End Sub

                    <TestMethod(), _
                      DataSource("System.Data.OleDb", _
                                 "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                 "PhysicsEngines", _
                                 DataAccessMethod.Sequential), _
                      DeploymentItem("TestCases.accdb")>
                    Public Sub Test_UnitScale_dg_cm()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        m_iSubSteps = 10

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Rotation", 0.05)
                        aryMaxErrors.Add("PendulumX", 0.05)
                        aryMaxErrors.Add("PendulumY", 0.05)
                        aryMaxErrors.Add("PendulumZ", 0.05)
                        aryMaxErrors.Add("default", 0.05)

                        m_strProjectName = "UnitScale_dg_cm"
                        m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Y", "-7 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge", "WorldPosition.Y", "0 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "L_7cm_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MotorStim", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "F_10mN_")

                    End Sub

                    Protected Overrides Sub AfterConversionBeforeSim()

                        If m_strPhysicsEngine = "Bullet" AndAlso m_iSubSteps > 1 Then
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsSubsteps", m_iSubSteps.ToString})
                        End If

                    End Sub

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

                    End Sub

                    <TestCleanup()> Public Overrides Sub MyTestCleanup()
                        MyBase.MyTestCleanup()
                    End Sub
#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
