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
                Public Class MeshConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"
                    '

                    <TestMethod(), _
                    DataSource("System.Data.OleDb", _
                               "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                               "MeshTestData", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_MeshConversion()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim strBlock1X As String = TestContext.DataRow("Block1X").ToString
                        Dim strBlock1Y As String = TestContext.DataRow("Block1Y").ToString
                        Dim strBlock1Z As String = TestContext.DataRow("Block1Z").ToString
                        Dim strBlock2X As String = TestContext.DataRow("Block2X").ToString
                        Dim strBlock2Y As String = TestContext.DataRow("Block2Y").ToString
                        Dim strBlock2Z As String = TestContext.DataRow("Block2Z").ToString
                        Dim strMesh1X As String = TestContext.DataRow("Mesh1X").ToString
                        Dim strMesh1Y As String = TestContext.DataRow("Mesh1Y").ToString
                        Dim strMesh1Z As String = TestContext.DataRow("Mesh1Z").ToString
                        Dim strBlock3X As String = TestContext.DataRow("Block3X").ToString
                        Dim strBlock3Y As String = TestContext.DataRow("Block3Y").ToString
                        Dim strBlock3Z As String = TestContext.DataRow("Block3Z").ToString
                        Dim strMeshType As String = TestContext.DataRow("MeshType").ToString

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("MeshX", 0.08)
                        aryMaxErrors.Add("MeshY", 0.08)
                        aryMaxErrors.Add("default", 0.08)

                        m_strProjectPath = "\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        ModifyMeshType(m_strOldProjectFolder, strMeshType)

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        aryMaxErrors("MeshX") = 0.03
                        aryMaxErrors("MeshY") = 0.03

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.X", strBlock1X})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.Y", strBlock1Y})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.Z", strBlock1Z})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block1_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.X", strBlock2X})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.Y", strBlock2Y})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.Z", strBlock2Z})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block2_")

                        aryMaxErrors("MeshX") = 0.08
                        aryMaxErrors("MeshY") = 0.08

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_3\Mesh", "LocalPosition.X", strMesh1X})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_3\Mesh", "LocalPosition.Y", strMesh1Y})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_3\Mesh", "LocalPosition.Z", strMesh1Z})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Mesh1_")

                        aryMaxErrors("MeshX") = 0.03
                        aryMaxErrors("MeshY") = 0.03

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.X", strBlock3X})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.Y", strBlock3Y})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_2\Right_Block", "LocalPosition.Z", strBlock3Z})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block3_")

                    End Sub


                    Protected Overridable Sub ModifyMeshType(ByVal strPath As String, _
                                                             ByVal strType As String)
                        Debug.WriteLine("ModifyMeshType. Path: '" & strPath & "', strType: " & strType)

                        Dim strFile As String = m_strRootFolder & strPath & "\Organism_1.astl"
                        Dim xnProject As New XmlDom()
                        xnProject.Load(strFile)

                        Dim xnStruct As XmlNode = xnProject.GetRootNode("Structure")
                        Dim xnRoot As XmlNode = xnProject.GetNode(xnStruct, "RigidBody")

                        Dim xnChild As XmlNode = xnProject.FindChildDataObject(xnRoot, "4177e00d-e941-45ee-b62a-f01f0ba16ef9")

                        xnProject.UpdateSingleNodeValue(xnChild, "CollisionMeshType", strType)

                        xnChild = xnProject.FindChildDataObject(xnChild, "6227ee72-b294-4e1b-ba5f-b4b47cd2ff85", False)

                        If Not xnChild Is Nothing Then
                            xnProject.UpdateSingleNodeValue(xnChild, "CollisionMeshType", strType)
                        End If

                        xnProject.Save(strFile)

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
