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
                Public Class MeshUITest
                    Inherits BodyPartUITest

#Region "Methods"

                    <TestMethod(), _
                     DataSource("System.Data.OleDb", _
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                "PhysicsEngines", _
                                DataAccessMethod.Sequential), _
                     DeploymentItem("TestCases.accdb")>
                    Public Sub Test_Mesh()
                        TestPart()
                    End Sub

                    Protected Overrides Sub ProcessExtraAddRootMethods(ByVal strPartType As String)

                        If strPartType = m_strPartType Then
                            'Wait for the collision mesh dialog to show, fill it in and hit ok
                            OpenDialogAndWait("Select Mesh", Nothing, Nothing)
                            ExecuteActiveDialogMethod("SetMeshParameters", New Object() {(m_strExecutableFolder & "\bin\Resources\" & m_strMeshFile), "Convex"})
                            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                            'Wait for the graphics mesh to show and hit ok.
                            OpenDialogAndWait("Select Mesh", Nothing, Nothing)
                            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        End If

                    End Sub

                    Protected Overrides Sub AfterAddChildPartJoint(ByVal strPartType As String, ByVal strJointType As String)

                        'Wait for the collision mesh dialog to show, fill it in and hit ok
                        OpenDialogAndWait("Select Mesh", Nothing, Nothing)
                        If m_strPhysicsEngine = "Vortex" Then
                            ExecuteActiveDialogMethod("SetMeshParameters", New Object() {(m_strExecutableFolder & "\bin\Resources\" & m_strMeshFile), "Triangular"})
                        Else
                            ExecuteActiveDialogMethod("SetMeshParameters", New Object() {(m_strExecutableFolder & "\bin\Resources\" & m_strMeshFile), "Convex"})
                        End If
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'Wait for the graphics mesh to show and hit ok.
                        OpenDialogAndWait("Select Mesh", Nothing, Nothing)
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                    End Sub

                    Protected Overrides Sub SimulateBeforeChildRemoved()

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "MeshFile", m_strExecutableFolder & "\bin\Resources\" & m_strSecondaryMeshFile})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterStruct_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "MeshFile", m_strExecutableFolder & "\bin\Resources\" & m_strMeshFile})

                    End Sub


#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strPartType = "Mesh"
                        m_strProjectName = "MeshTest"
                        m_strProjectPath = "\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_bTestTexture = False

                        m_iInitialZoomDist2 = 150
                        m_iInitialZoomDist2 = 150

                        m_iSecondaryZoomDist2 = 150
                        m_iSecondaryZoomDist2 = 150

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
