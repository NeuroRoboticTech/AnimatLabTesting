﻿Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace BodyEditorTests
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class TerrainUITest
                    Inherits BodyPartUITest

#Region "Properties"

                    Protected Overrides ReadOnly Property HasRootGraphic() As Boolean
                        Get
                            Return False
                        End Get
                    End Property

                    Protected Overrides ReadOnly Property AllowRootRotations() As Boolean
                        Get
                            Return False
                        End Get
                    End Property

#End Region

#Region "Methods"

                    <TestMethod(), _
                     DataSource("System.Data.OleDb", _
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                "PhysicsEngines", _
                                DataAccessMethod.Sequential), _
                     DeploymentItem("TestCases.accdb")>
                    Public Sub Test_Terrain()
                        TestPart()
                    End Sub

                    Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                        MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                        TestSettingHeightMap(strStructure, strPart)

                        'Set the SegmentWidth to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentWidth", "0.2"})

                        'Set the SegmentWidth to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentWidth", "0"}, "The segment width must be greater than zero.")

                        'Set the SegmentWidth to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentWidth", "-0.2"}, "The segment width must be greater than zero.")

                        'Set the SegmentWidth to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentWidth", "0.1"})


                        'Set the SegmentLength to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentLength", "0.2"})

                        'Set the SegmentLength to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentLength", "0"}, "The segment length must be greater than zero.")

                        'Set the SegmentLength to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentLength", "-0.2"}, "The segment length must be greater than zero.")

                        'Set the SegmentLengthto a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "SegmentLength", "0.1"})


                        'Set the MaxHeight to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MaxHeight", "1"})

                        'Set the MaxHeight to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MaxHeight", "0"}, "The maximum height must be greater than zero.")

                        'Set the MaxHeight to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MaxHeight", "-0.2"}, "The maximum height must be greater than zero.")

                        'Set the MaxHeight a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MaxHeight", "0.5"})


                        ''For the terrain part we need to do an additional rotation so we can see the axis.
                        'DragMouse(m_ptTerrainAxisViewStart, m_ptTerrainAxisViewEnd, MouseButtons.Left, ModifierKeys.None, True)

                    End Sub

                    Protected Overrides Sub ProcessExtraAddRootMethods(ByVal strPartType As String)

                        If strPartType = m_strPartType Then
                            'Wait for the collision mesh dialog to show, fill it in and hit ok
                            OpenDialogAndWait("Select Terrain", Nothing, Nothing)
                            ExecuteActiveDialogMethod("SetTerrainParameters", New Object() {(m_strExecutableFolder & "\bin\Resources\TerrainTest_HeightMap.jpg"), (m_strExecutableFolder & "\bin\Resources\Bricks.bmp"), 0.1, 0.1, 0.5})
                            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        End If

                    End Sub

                    Protected Overrides Sub RepositionChildPart()

                        'Reposition the child part relative to the landscape part
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "LocalPosition.Z", "0.5"})

                    End Sub

                    Protected Overrides Sub RepositionStruct1BeforeSim()
                        'Reposition the structure
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Z", "0"})
                    End Sub

                    Protected Overrides Sub RepositionStruct2BeforeSim()
                        'Move the structure up.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2", "WorldPosition.Y", "1"})

                        'Resize the sphere
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Radius", "0.05"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root\Root_Graphics", "Radius", "0.05"})
                    End Sub


                    Protected Overrides Sub SimulateBeforeChildRemoved()

                        'Set the texture to an valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "MeshFile", _
                                                                                    (m_strExecutableFolder & "\bin\Resources\Small_Offset_Bowl_Heightfield.jpg")})
                        'Reposition the structure
                        If m_strPhysicsEngine = "Bullet" Then
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.X", "-60 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Y", "18 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Z", "1.5 c"})
                        Else
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.X", "-60 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Y", "50 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Z", "1.5 c"})
                        End If

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "Hole1_")

                        'Set the texture to an valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "MeshFile", _
                                                                                    (m_strExecutableFolder & "\bin\Resources\Small_Offset_Bowl_Heightfield_2.jpg")})
                        'Reposition the structure
                        If m_strPhysicsEngine = "Bullet" Then
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.X", "32 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Y", "20 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Z", "-1.15 c"})
                        Else
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.X", "32 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Y", "50 c"})
                            ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\Structure_2\Body Plan\Root", "WorldPosition.Z", "-1.15 c"})
                        End If

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "Hole2_")

                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strPartType = "Terrain"
                        m_strProjectName = "TerrainTest"
                        m_strSecondaryPartType = "Box"
                        m_strChildJoint = "Hinge"
                        m_strStruct2RootPart = "Sphere"

                        m_bTestTexture = True
                        m_strTextureFile = "TerrainTest_TextureMap.jpg"
                        m_strMeshFile = "TerrainTest_HeightMap2.jpg"
                        m_bTestDensity = False

                        m_strProjectPath = "\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_strAmbient = "#AAAAAA"

                        m_iInitialZoomDist1 = 150
                        m_iInitialZoomDist2 = 0

                        m_ptTranslateXAxisStart = New Point(866, 396)
                        m_ptTranslateXAxisEnd = New Point(1065, 399)

                        m_ptChildTranslateZAxisStart = New Point(905, 395)
                        m_ptChildTranslateZAxisEnd = New Point(645, 398)

                        m_strMoveChildLocalYAxis = "Z"
                        m_strMoveChildLocalZAxis = "Y"

                        m_dblMinTranChildLocalZ = -2
                        m_dblMaxTranChildLocalZ = -0.05

                        m_ptChildRotateYAxisStart = New Point(779, 281)
                        m_ptChildRotateYAxisEnd = New Point(693, 136)

                        m_ptChildRotateZAxisStart = New Point(775, 403)
                        m_ptChildRotateZAxisEnd = New Point(931, 413)

                        m_dblMinRotChildY = -90
                        m_dblMaxRotChildY = -10

                        m_dblManChildWorldLocalZTest = -1
                        m_dblManChildLocalWorldZTest = -2

                        m_dblStructChildRotateTestYX = 2.828
                        m_dblStructChildRotateTestYY = -0.000412
                        m_dblStructChildRotateTestYZ = -2.0

                        m_dblStructChildRotateTestZX = -0.000005119
                        m_dblStructChildRotateTestZY = 1.999
                        m_dblStructChildRotateTestZZ = -2.829


                        m_dblStructJointRotateTestYX = 2.828
                        m_dblStructJointRotateTestYY = -0.000433
                        m_dblStructJointRotateTestYZ = -2.1

                        m_dblStructJointRotateTestZX = -0.070716
                        m_dblStructJointRotateTestZY = 1.999
                        m_dblStructJointRotateTestZZ = -2.9

                        m_ptClickToAddChild = New Point(800, 200)

                        CleanupProjectDirectory()
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
