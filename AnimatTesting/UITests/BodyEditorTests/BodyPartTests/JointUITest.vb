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

Namespace UITests
    Namespace BodyEditorTests
        Namespace BodyPartTests

            <CodedUITest()>
            Public MustInherit Class JointUITest
                Inherits AnimatUITest

#Region "Attributes"

                Protected m_strPartType As String = "Box"
                Protected m_strSecondaryPartType As String = ""
                Protected m_strJointType As String = "Hinge"

                Protected m_strAddArmPath As String = ""
                Protected m_strAddRootAttach As String = ""
                Protected m_strAddArmAttach As String = ""

                Protected m_ptAddArmClick As New Point(751, 362)
                Protected m_ptZoomStart As New Point(877, 100)
                Protected m_iZoom1 As Integer = 300
                Protected m_iZoom2 As Integer = 300

                Protected m_ptRotateArmStart As New Point(100, 420)
                Protected m_iRotateArm1 As Integer = 700
                Protected m_iRotateArm2 As Integer = 700

                Protected m_ptRotateArm2Start As New Point(1196, 135)
                Protected m_ptRotateArm2End As New Point(1064, 288)

                Protected m_ptAddRootAttach As New Point(757, 330)
                Protected m_ptAddArmAttach As New Point(990, 545)

                Protected m_strMoveArmWorldZAxis As String = "Z"
                Protected m_strMoveArmLocalZAxis As String = "Z"
                Protected m_ptTranslateZAxisStart As New Point(781, 642)  'Point(790, 634)
                Protected m_ptTranslateZAxisEnd As New Point(700, 725) 'Point(741, 669)
                Protected m_dblMinTranArmWorldZ As Double = 0
                Protected m_dblMaxTranArmWorldZ As Double = 2
                Protected m_dblMinTranArmStructZ As Double = 0
                Protected m_dblMaxTranArmStructZ As Double = 0
                Protected m_dblMinTranArmLocalZ As Double = 0
                Protected m_dblMaxTranArmLocalZ As Double = 2

                Protected m_strMoveJointWorldYAxis As String = "Y"
                Protected m_strMoveJointLocalYAxis As String = "Y"
                Protected m_ptTransJointYAxisStart As New Point(790, 634)
                Protected m_ptTransJointYAxisEnd As New Point(741, 669)
                Protected m_dblMinTranJointWorldY As Double = 0
                Protected m_dblMaxTranJointWorldY As Double = 2
                Protected m_dblMinTranJointStructY As Double = 0
                Protected m_dblMaxTranJointStructY As Double = 0
                Protected m_dblMinTranJointLocalY As Double = 0
                Protected m_dblMaxTranJointLocalY As Double = 2

                Protected m_strInitialJointXPos As String = "-0.125"
                Protected m_strInitialJointYPos As String = "0"
                Protected m_strInitialJointZPos As String = "0"

                Protected m_strInitialJointXRot As String = "90"
                Protected m_strInitialJointYRot As String = "0"
                Protected m_strInitialJointZRot As String = "90"

                Protected m_strNoMoveJointRot As String = m_strInitialJointXRot

                Protected m_ptMoveJoint1Start As New Point(687, 428)
                Protected m_ptMovejoint1End As New Point(652, 608)

                Protected m_ptRotateJoint1Start As New Point(687, 428)
                Protected m_ptRotatejoint1End As New Point(652, 608)

                Protected m_strFallUpper1 As String = "45"
                Protected m_strFallUpper2 As String = "90"
                Protected m_strFallUpper3 As String = "25"

                Protected m_strFallLower1 As String = "-45"
                Protected m_strFallLower2 As String = "-90"
                Protected m_strFallLower3 As String = "-25"

                Protected m_dblMaxMovePos As Double = 15.09315
                Protected m_dblMaxMovePosError As Double = 0.2

                Protected m_dblMaxMoveVel As Double = 5.918866
                Protected m_dblMaxMoveVelError As Double = 0.3

                Protected m_dblMaxRotPos As Double = 0.08810297
                Protected m_dblMaxRotPosError As Double = 0.01

                Protected m_strForceXJointRotation As String = "0"

                Protected m_dblMouseRotateJointMin As Double = -360
                Protected m_sblMouseRotateJointMax As Double = -90

                Protected m_aryMaxJointErrors As New Hashtable

#End Region

#Region "Properties"

                Public Overridable ReadOnly Property IsMotorizedJoint() As Boolean
                    Get
                        Return True
                    End Get
                End Property

#End Region

#Region "Methods"

                Protected Overridable Sub TestJoint()

                    If Not TestContext.DataRow Is Nothing Then
                        m_strPhysicsEngine = TestContext.DataRow("Physics").ToString
                        Dim bEnabled As Boolean = CBool(TestContext.DataRow("Enabled"))
                        If Not bEnabled Then Return
                    Else
                        m_strPhysicsEngine = "Vortex"
                    End If

                    StartNewProject()

                    m_strAddArmPath = "Simulation\Environment\" & m_strStructureGroup & _
                                       "\" & m_strStruct1Name & "\Body Plan\Root"

                    m_strAddRootAttach = "Simulation\Environment\" & m_strStructureGroup & _
                                       "\" & m_strStruct1Name & "\Body Plan\Root"
                    m_strAddArmAttach = "Simulation\Environment\" & m_strStructureGroup & _
                                       "\" & m_strStruct1Name & "\Body Plan\Root"


                    'Create the test armature.
                    CreateArmature(m_strPartType, m_strSecondaryPartType, m_strJointType, _
                                   m_strAddArmPath, m_ptZoomStart, m_iZoom1, m_iZoom2, _
                                    False, "Attachment", m_strAddRootAttach, m_strAddArmAttach)

                    'Create the chart for the test armature
                    CreateArmatureChart(False)

                    SimulateJointTests()

                End Sub

                Protected Overridable Sub SetMaxErrors()
                    m_aryMaxJointErrors.Clear()
                    m_aryMaxJointErrors.Add("Time", 0.001)
                    m_aryMaxJointErrors.Add("Arm_X", 0.1)
                    m_aryMaxJointErrors.Add("Arm_Y", 0.1)
                    m_aryMaxJointErrors.Add("Arm_Z", 0.1)
                    m_aryMaxJointErrors.Add("JointVelocity", 0.2)
                    m_aryMaxJointErrors.Add("JointPosition", 0.1)
                    m_aryMaxJointErrors.Add("Rotation", 0.2)
                    m_aryMaxJointErrors.Add("default", 0.1)
                End Sub

                Protected Overrides Sub RecalculatePositionsUsingResolution()
                    MyBase.RecalculatePositionsUsingResolution()

                    m_ptRotateArmStart.X = CInt(m_ptRotateArmStart.X * m_dblResScaleWidth)
                    m_ptRotateArmStart.Y = CInt(m_ptRotateArmStart.Y * m_dblResScaleHeight)

                    m_ptRotateArm2Start.X = CInt(m_ptRotateArm2Start.X * m_dblResScaleWidth)
                    m_ptRotateArm2Start.Y = CInt(m_ptRotateArm2Start.Y * m_dblResScaleHeight)

                    m_ptRotateArm2End.X = CInt(m_ptRotateArm2End.X * m_dblResScaleWidth)
                    m_ptRotateArm2End.Y = CInt(m_ptRotateArm2End.Y * m_dblResScaleHeight)

                    m_ptAddRootAttach.X = CInt(m_ptAddRootAttach.X * m_dblResScaleWidth)
                    m_ptAddRootAttach.Y = CInt(m_ptAddRootAttach.Y * m_dblResScaleHeight)

                    m_ptAddArmAttach.X = CInt(m_ptAddArmAttach.X * m_dblResScaleWidth)
                    m_ptAddArmAttach.Y = CInt(m_ptAddArmAttach.Y * m_dblResScaleHeight)

                    m_ptTranslateZAxisStart.X = CInt(m_ptTranslateZAxisStart.X * m_dblResScaleWidth)
                    m_ptTranslateZAxisStart.Y = CInt(m_ptTranslateZAxisStart.Y * m_dblResScaleHeight)

                    m_ptTranslateZAxisEnd.X = CInt(m_ptTranslateZAxisEnd.X * m_dblResScaleWidth)
                    m_ptTranslateZAxisEnd.Y = CInt(m_ptTranslateZAxisEnd.Y * m_dblResScaleHeight)

                    m_ptRotateJoint1Start.X = CInt(m_ptRotateJoint1Start.X * m_dblResScaleWidth)
                    m_ptRotateJoint1Start.Y = CInt(m_ptRotateJoint1Start.Y * m_dblResScaleHeight)

                    m_ptRotatejoint1End.X = CInt(m_ptRotatejoint1End.X * m_dblResScaleWidth)
                    m_ptRotatejoint1End.Y = CInt(m_ptRotatejoint1End.Y * m_dblResScaleHeight)

                    m_ptMoveJoint1Start.X = CInt(m_ptMoveJoint1Start.X * m_dblResScaleWidth)
                    m_ptMoveJoint1Start.Y = CInt(m_ptMoveJoint1Start.Y * m_dblResScaleHeight)

                    m_ptMovejoint1End.X = CInt(m_ptMovejoint1End.X * m_dblResScaleWidth)
                    m_ptMovejoint1End.Y = CInt(m_ptMovejoint1End.Y * m_dblResScaleHeight)

                    m_ptMoveJoint1Start.X = CInt(m_ptMoveJoint1Start.X * m_dblResScaleWidth)
                    m_ptMoveJoint1Start.Y = CInt(m_ptMoveJoint1Start.Y * m_dblResScaleHeight)

                    m_ptTransJointYAxisStart.X = CInt(m_ptTransJointYAxisStart.X * m_dblResScaleWidth)
                    m_ptTransJointYAxisEnd.Y = CInt(m_ptTransJointYAxisEnd.Y * m_dblResScaleHeight)

                End Sub

                Protected Overrides Sub RepositionChildPart()

                    'ZoomInOnPart(m_ptRotateArmStart, m_iRotateArm1, m_iRotateArm2, False, MouseButtons.Left)

                    'DragMouse(m_ptRotateArm2Start, m_ptRotateArm2End, MouseButtons.Left, ModifierKeys.None, True)

                    'Set the root part to be frozen.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Freeze", "True"})

                    'Resize the root part and graphic.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Height", "0.2"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Length", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Height", "0.2"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Length", "0.05"})

                    'Resize the child part and graphic.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Height", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Length", "0.2"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Height", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Length", "0.2"})

                    'Reposition the child part relative to the parent part
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.X", "0.125"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Y", "-0.075"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Z", "0"})

                    'Reposition the joint relative to the parent part
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.X", m_strInitialJointXPos})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.Y", m_strInitialJointYPos})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.Z", m_strInitialJointZPos})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strInitialJointXRot})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Y", m_strInitialJointYRot})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Z", m_strInitialJointZRot})

                End Sub

                Protected Overrides Sub RepositionBlockerPart()
                    'Resize the child part and graphic.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Height", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Length", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Height", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Length", "0.05"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "0.125"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.X", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.Y", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.Z", "0"})
                End Sub

                Protected Overridable Sub SimulateJointTests()
                    SetMaxErrors()
                    TestConstraintLimitsByFalling()
                    TestConstraintLimitsWithMotor()
                    TestConstraintLimitsWithForce()
                End Sub

                Protected Overridable Sub TestConstraintLimitsByFalling()
                    Debug.WriteLine("*********** TestConstraintLimitsByFalling **************")

                    'First simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallUp" & m_strFallUpper1 & "Deg_")

                    'Now increase upper limit
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "UpperLimit.LimitPos", m_strFallUpper2})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "FallUp" & m_strFallUpper2 & "Deg_")

                    'Now decrease upper limit
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "UpperLimit.LimitPos", m_strFallUpper3})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "FallUp" & m_strFallUpper3 & "Deg_")

                    'reset upper limit, rotate body to test lower limit
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "UpperLimit.LimitPos", m_strFallUpper1})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.X", "180"})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallLow" & m_strFallLower1 & "Deg_")

                    'Now increase upper limit
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LowerLimit.LimitPos", m_strFallLower2})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallLow" & m_strFallLower2 & "Deg_")

                    'Now decrease upper limit
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LowerLimit.LimitPos", m_strFallLower3})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallLow" & m_strFallLower3 & "Deg_")

                    'Reset the limit, rotate the part so it should not fall.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LowerLimit.LimitPos", m_strFallLower1})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.X", m_strNoMoveJointRot})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallNone_")

                    'Now Rotate joint so it should fall.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", "0"})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallXRot0_")

                    'Now Rotate joint so it should fall.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", "45"})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallXRot45_")

                    'Turn off constraint limits
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "EnableLimits", "False"})

                    'simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "FallXRot45NoLimit_")

                    'Reset armature
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "EnableLimits", "True"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strInitialJointXRot})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.X", "0"})

                    'Reposition the blocker to be in the way
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0.2"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "-0.18"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "BlockFall45_")

                    'Made the blocker wider.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Width", "0.3"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Width", "0.3"})

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", False})

                    ''Now move the arm over using the mouse.
                    'MovePartAxis(m_strStruct1Name, "Root\Joint_1\Arm", _
                    '             m_strMoveArmWorldZAxis, m_strMoveArmLocalZAxis, _
                    '             m_ptTranslateZAxisStart, m_ptTranslateZAxisEnd, _
                    '             m_dblMinTranArmWorldZ, m_dblMaxTranArmWorldZ, _
                    '             m_dblMinTranArmStructZ, m_dblMaxTranArmStructZ, _
                    '             m_dblMinTranArmLocalZ, m_dblMaxTranArmLocalZ)

                    'RunSimulationWaitToEnd()

                    ''No prefix on the exported chart.
                    'LoadDataChart(m_strRootFolder & m_strTestDataPath, "DataTool_1.txt", "MouseMove_")
                    'CompareColummData(4, 150, 180, enumDataComparisonType.Max, m_dblMaxMoveVel, 0, m_dblMaxMoveVelError)
                    'CompareColummData(5, 150, 180, enumDataComparisonType.Max, m_dblMaxMovePos, 0, m_dblMaxMovePosError)

                    'Reset blocker position
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "0.125"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                    'Reset blocker size.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Width", "0.05"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Width", "0.05"})

                    'Reset the arm position.
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Z", "0"})

                End Sub

                Protected Overridable Sub TestConstraintLimitsWithMotor()
                    Debug.WriteLine("*********** TestConstraintLimitsWithMotor **************")

                    'If this motor cannot be motorized then skip this test.
                    If Not Me.IsMotorizedJoint Then Return

                    If m_strPhysicsEngine = "Bullet" Then
                        'The motors do not currently work well at lower unit scales unless you subdivide your time steps.
                        'I plan to fix this later, but for now do this to get it to work.
                        'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsSubsteps", "10"})

                        'Cut down the joint resistance
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Friction.Coefficient", "0.002"})
                    End If

                    ''Add motor velocity to joint. 
                    AddStimulus("Motor Velocity", m_strStruct1Name, "\Body Plan\Root\Joint_1", "JointVelocity") ', "Stimulus_2"
                    SetMotorVelocityStimulus("JointVelocity", False, True, 0, 5, True, True, 1, "")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorUpVel1_")

                    SetMotorVelocityStimulus("JointVelocity", False, True, 1, 5, True, True, -1, "")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorDownVel1_")

                    SetMotorVelocityStimulus("JointVelocity", False, True, 0, 5, False, True, 2, "")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorUpVel2_")

                    SetMotorVelocityStimulus("JointVelocity", True, True, 1, 5, False, True, -2, "")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorDownVel2_")

                    SetMotorVelocityStimulus("JointVelocity", False, True, 1, 5, False, False, 0, "-5*t")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorDownVelEqu_")

                    'Rotate joint
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Z", "45"})

                    SetMotorVelocityStimulus("JointVelocity", False, True, 1, 5, False, False, 0, "-0.25*t")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorDownVelEqu45Deg_")

                    'Reset the joint
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Z", m_strInitialJointZRot})

                    DeletePart("Stimuli\JointVelocity", "Delete Stimulus")

                    If m_strPhysicsEngine = "Bullet" Then
                        'The motors do not currently work well at lower unit scales unless you subdivide your time steps.
                        'I plan to fix this later, but for now do this to get it to work.
                        'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsSubsteps", "1"})

                        'Cut down the joint resistance
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Friction.Coefficient", "0.02"})
                    End If
                End Sub

                Protected Overridable Sub TestConstraintLimitsWithForce()
                    Debug.WriteLine("*********** TestConstraintLimitsWithForce **************")

                    If m_strPhysicsEngine = "Bullet" Then
                        'The joint friction force in bullet is stronger than in vortex, so lets adjust it down a little for the tests
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Friction.Coefficient", "0.5 m"})
                    End If

                    'Add force stimulus to child part. 
                    AddStimulus("Force", m_strStruct1Name, "\Body Plan\Root\Joint_1\Arm", "ArmForce") ', "Stimulus_3"
                    SetForceStimulus("ArmForce", False, True, 1, 2, 0, 0, 0, 0, 10, 0, 0, 0, 0)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorUpForce10_")

                    SetForceStimulus("ArmForce", False, True, 1, 2, 0, 0, 0, 0, 15, 0, 0, 0, 0)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorUpForce15_")

                    'Rotate joint
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strForceXJointRotation})

                    SetForceStimulus("ArmForce", False, True, 1, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0)

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, m_aryMaxJointErrors, "MotorLeftForce1_")

                    'Reset the joint
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strInitialJointXRot})

                    'ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", False})
                    'RotatePartAxis(m_strStruct1Name, "Root\Joint_1", "X", m_ptRotateJoint1Start, m_ptRotatejoint1End, m_dblMouseRotateJointMin, m_sblMouseRotateJointMax, False)
                    'SetForceStimulus("ArmForce", False, True, 0, 15, 0, 0, 0, 0, 0, 5, 0, 0, 0)

                    'RunSimulationWaitToEnd()
                    'LoadDataChart(m_strRootFolder & m_strTestDataPath, "DataTool_1.txt", "MouseRotate_")
                    'CompareColummData(3, 3070, 3080, enumDataComparisonType.Max, m_dblMaxRotPos, 0, m_dblMaxRotPosError)

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strInitialJointXRot})

                    DeletePart("Stimuli\ArmForce", "Delete Stimulus")
                End Sub


#End Region

            End Class

        End Namespace
    End Namespace
End Namespace