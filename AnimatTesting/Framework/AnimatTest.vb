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
Imports System.Reflection
Imports System.Xml

Namespace Framework

    Public MustInherit Class AnimatTest

#Region " Enums "

        Public Enum enumErrorTextType
            Equals
            Contains
            BeginsWith
            EndsWith
        End Enum

        Public Enum enumDataComparisonType
            WithinRange
            Average
            Max
            Min
        End Enum

#End Region

#Region "Attributes"

        Protected Shared m_iPort As Integer = 8080
        Protected Shared m_oServer As AnimatServer.Server
        Protected Shared m_tcpChannel As TcpChannel
        Protected Shared m_strRootFolder As String
        Protected Shared m_strExecutableFolder As String
        Protected Shared m_bGenerateTempates As Boolean = False
        Protected Shared m_bAttachServerOnly As Boolean = False
        Protected m_strProjectName As String = ""
        Protected m_strProjectPath As String = ""
        Protected m_strTestDataPath As String = ""
        Protected m_dblSimEndTime As Double = 15
        Protected m_dblChartEndTime As Double = 8
        Protected m_bIgnoreSimAndCompare As Boolean = False
        Protected m_strOldProjectFolder As String
        Protected m_strPhysicsEngine As String = "Bullet" '"Vortex"
        Protected m_strDistanceUnits As String = "Decimeters"
        Protected m_strMassUnits As String = "Grams"

#End Region

#Region "Mehods"

        Protected Shared Sub InitializeConfiguration()
            Debug.WriteLine("Initializing Configuration")

            m_strRootFolder = System.Configuration.ConfigurationManager.AppSettings("RootFolder")
            If m_strRootFolder Is Nothing OrElse m_strRootFolder.Trim.Length = 0 Then
                Throw New System.Exception("Root Folder path was not found in configuration file.")
            End If

            m_strExecutableFolder = System.Configuration.ConfigurationManager.AppSettings("ExecutableFolder")
            If m_strExecutableFolder Is Nothing OrElse m_strExecutableFolder.Trim.Length = 0 Then
                Throw New System.Exception("Executable Folder path was not found in configuration file.")
            End If

            m_bGenerateTempates = CType(System.Configuration.ConfigurationManager.AppSettings("GenerateTemplates"), Boolean)
            m_bAttachServerOnly = CType(System.Configuration.ConfigurationManager.AppSettings("AttachServerOnly"), Boolean)

            Debug.WriteLine("Root Folder: " & m_strRootFolder)
            Debug.WriteLine("Generate Tempates: " & m_bGenerateTempates)
            Debug.WriteLine("Attach Server Only: " & m_bAttachServerOnly)

        End Sub

        Protected Overridable Sub StartApplication(ByVal strProject As String, Optional ByVal bAttachOnly As Boolean = False)
            'Get a new port number each time we spin up a new independent test.
            m_iPort = Util.GetNewPort()
 
            'Start the application.
            StartApplication(strProject, m_iPort, bAttachOnly)
        End Sub

        Protected Overridable Sub StartApplication(ByVal strProject As String, ByVal iPort As Integer, Optional ByVal bAttachOnly As Boolean = False)

            If Not bAttachOnly Then
                Debug.WriteLine("Starting application on port: " & iPort)

                'First just try and attach to an existing app. If one is not found then start the exe.
                If AttachServer(iPort, False) Then
                    Return
                End If

                Dim strArgs = ""
                If strProject.Trim.Length > 0 Then
                    strArgs = " -Project " & strProject
                End If
                strArgs = strArgs & " -Port " & iPort.ToString

                Process.Start(m_strExecutableFolder & "\bin\AnimatLab2.exe", strArgs)
                'Process.Start("C:\Program Files (x86)\NeuroRobotic Technologies\AnimatLab\2.0.7\bin\AnimatLab2.exe", strArgs)

                Threading.Thread.Sleep(3000)
            End If

            AttachServer(iPort, True)

        End Sub

        Protected Overridable Function AttachServer(ByVal iPort As Integer, ByVal bRepeatAttempt As Boolean) As Boolean

            Debug.WriteLine("Attempting to attach to server on port: " & iPort)

            Dim props As IDictionary = New Hashtable()
            props("name") = "AnimatLab:" & iPort

            m_tcpChannel = New TcpChannel(props, Nothing, Nothing)
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(m_tcpChannel, True)
            For iTry As Integer = 0 To 3
                If GetAnimatServer(iPort) Then
                    Debug.WriteLine("Successfully attached to server on port: " & iPort)
                    Return True
                Else
                    Debug.WriteLine("Failed to attached to server on port: " & iPort & ", Attempt: " & iTry)
                    If Not bRepeatAttempt Then
                        DetachServer()
                        Return False
                    End If
                    Threading.Thread.Sleep(3000)
                End If
            Next

            DetachServer()
            Return False
        End Function

        Protected Overridable Function GetAnimatServer(ByVal iPort As Integer) As Boolean
            Try
                m_oServer = DirectCast(Activator.GetObject(GetType(AnimatServer.Server), "tcp://localhost:" & iPort & "/AnimatLab"), AnimatServer.Server)
                'Make sure we can actually communicate.
                m_oServer.GetProperty("SimIsRunning")
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Protected Overridable Sub DetachServer()
            If Not m_tcpChannel Is Nothing Then
                Debug.WriteLine("Detaching from server")
                System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(m_tcpChannel)
            End If
        End Sub

        Protected Overridable Sub CleanupProjectDirectory()
            'Make sure any left over project directory is cleaned up before starting the test.
            If m_strRootFolder.Length > 0 AndAlso m_strProjectPath.Length > 0 AndAlso m_strProjectName.Length > 0 Then
                Debug.WriteLine("Cleaning up the project directory")
                DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
            End If
        End Sub

        Protected Overridable Sub CleanupConversionProjectDirectory()
            Debug.WriteLine("Cleaning up the conversion project directory")

            'Make sure any left over project directory is cleaned up before starting the test.
            If m_strRootFolder.Length > 0 AndAlso m_strProjectPath.Length > 0 AndAlso m_strProjectName.Length > 0 Then
                DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
            End If

            'Copy the old version project folder back so we can load it up.
            If m_strOldProjectFolder.Length > 0 Then
                Util.CopyDirectory(m_strRootFolder & m_strOldProjectFolder, m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
            End If
        End Sub

        Protected Overridable Function SetPhysicsEngine(ByVal dataRow As System.Data.DataRow) As Boolean
            m_strPhysicsEngine = dataRow("Physics").ToString
            Debug.WriteLine("Testing physics engine '" & m_strPhysicsEngine & "'")
            Return CBool(dataRow("Enabled"))
        End Function

        Protected Overridable Sub SetPhysicsEngineOnExistingProject(ByVal strPath As String, ByVal strPhysicsEngine As String)
            Debug.WriteLine("SetPhysicsEngineOnExistingProject. Path: '" & strPath & "', strPhysicsEngine: " & strPhysicsEngine)

            Try
                Dim xnProject As New XmlDom()
                xnProject.Load(strPath)

                Dim xnProj As XmlNode = xnProject.GetRootNode("Project")
                xnProject.UpdateSingleNodeValue(xnProj, "Physics", strPhysicsEngine)

                xnProject.Save(strPath)
            Catch ex As Exception
                Debug.Write(ex.Message)
            End Try

        End Sub

        Protected Overridable Sub WaitWhileBusy(Optional ByVal bSkipWaiting As Boolean = False, Optional ByVal bErrorOk As Boolean = False)
            If bSkipWaiting Then Return

            Dim bInProgress As Boolean = DirectCast(GetApplicationProperty("AutomationMethodInProgress"), Boolean)
            Dim bIsBusy As Boolean = DirectCast(GetApplicationProperty("AppIsBusy"), Boolean)
            Dim iCount = 0

            While bInProgress OrElse bIsBusy
                Debug.WriteLine("Waiting on automation in progress. Count: " & iCount)
                Threading.Thread.Sleep(20)
                bInProgress = DirectCast(GetApplicationProperty("AutomationMethodInProgress"), Boolean)
                bIsBusy = DirectCast(GetApplicationProperty("AppIsBusy"), Boolean)

                If bInProgress OrElse bIsBusy Then
                    CheckForErrorDialog(bErrorOk)

                    iCount = iCount + 1
                    If iCount > 1000 Then
                        Throw New System.Exception("Timed out waiting for an automation method in progress.")
                    End If
                End If
            End While

            Threading.Thread.Sleep(100)

        End Sub

        Protected Overridable Sub CheckForErrorDialog(ByVal bErrorOk As Boolean)
            Dim oVal As Object = GetApplicationProperty("ErrorDialogMessage")
            If Not oVal Is Nothing AndAlso CStr(oVal).Length > 0 Then
                If bErrorOk Then
                    Return
                Else
                    Throw New System.Exception("Error dialog open. Message: " & oVal.ToString)
                End If
            End If
        End Sub

        Protected Overridable Function ExecuteMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("Executing Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteIndirectMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 20, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("Executing Indirect Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteIndirectMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)

            'Check to see if an error dialog is present. If it is then get the error name.
            Dim strFormName As String = DirectCast(ExecuteDirectMethod("ActiveDialogName", Nothing, 200, bErrorOk, bSkipWaiting), String)
            If strFormName = "Error" Then
                Dim strError As String = CStr(GetApplicationProperty("ErrorDialogMessage"))
                Throw New System.Exception(strError)
            End If

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Public Overridable Function ExecuteIndirectMethodOnObject(ByVal strPath As String, ByVal strMethod As String, ByVal aryInnerParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 20, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("ExecuteIndirectMethodOnObject: Path: '" & strPath & ", Method: " & strMethod & ", Params: '" & Util.ParamsToString(aryInnerParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryParams As Object() = New Object() {strPath, strMethod, aryInnerParams}

            Dim oRet As Object = ExecuteDirectMethod("ExecuteIndirectMethodOnObject", aryParams)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            Return oRet
        End Function

        Public Overridable Function ExecuteAppPropertyMethod(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryInnerParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("ExecuteAppPropertyMethod: Path: '" & strPropertyName & ", Method: " & strMethodName & ", Params: '" & Util.ParamsToString(aryInnerParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryParams As Object() = New Object() {strPropertyName, strMethodName, aryInnerParams}

            Dim oRet As Object = ExecuteDirectMethod("ExecuteAppPropertyMethod", aryParams)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            Return oRet
        End Function

        Public Overridable Function ExecuteIndirectAppPropertyMethod(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryInnerParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 20, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("ExecuteIndirectAppPropertyMethod: Path: '" & strPropertyName & ", Method: " & strMethodName & ", Params: '" & Util.ParamsToString(aryInnerParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryParams As Object() = New Object() {strPropertyName, strMethodName, aryInnerParams}

            Dim oRet As Object = ExecuteDirectMethod("ExecuteIndirectAppPropertyMethod", aryParams)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            Return oRet
        End Function

        Protected Overridable Sub ExecuteMethodAssertError(ByVal strMethodName As String, ByVal aryParams() As Object, ByVal strErrorText As String, _
                                                           Optional ByVal eErrorTextType As enumErrorTextType = enumErrorTextType.Contains, _
                                                           Optional ByVal iWaitMilliseconds As Integer = 20)

            Try
                Debug.WriteLine("ExecuteMethodAssertError Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds & ", ErrorText: '" & strErrorText & "', ErrorTextType: '" & eErrorTextType.ToString & "'")


                Dim oRet As Object = m_oServer.ExecuteIndirectMethod(strMethodName, aryParams)

                AssertErrorDialogShown(strErrorText, eErrorTextType)

            Catch ex As System.Exception
                Debug.WriteLine("Exception was caught.")

                If Not ex.InnerException Is Nothing Then
                    CheckException(ex.InnerException, strErrorText, eErrorTextType)
                Else
                    Debug.WriteLine("No inner execption was found.")
                    Throw ex
                End If
            End Try
        End Sub


        Protected Overridable Sub CheckException(ByVal ex As Exception, ByVal strErrorText As String, _
                                                           Optional ByVal eErrorTextType As enumErrorTextType = enumErrorTextType.Contains)
            If Not ex Is Nothing Then
                If eErrorTextType = enumErrorTextType.Contains AndAlso ex.Message.Contains(strErrorText) Then
                    Debug.WriteLine("It matched the text")
                    Return
                ElseIf eErrorTextType = enumErrorTextType.BeginsWith AndAlso ex.Message.StartsWith(strErrorText) Then
                    Debug.WriteLine("It matched the text")
                    Return
                ElseIf eErrorTextType = enumErrorTextType.EndsWith AndAlso ex.Message.EndsWith(strErrorText) Then
                    Debug.WriteLine("It matched the text")
                    Return
                Else
                    Debug.WriteLine("It did not match the text. Message: '" & ex.Message & "'")
                    Throw ex
                End If
            Else
                Debug.WriteLine("No execption was found.")
                Throw ex
            End If
        End Sub

        Protected Overridable Sub OpenDialogAndWait(ByVal strDlgName As String, ByVal oActionMethod As MethodInfo, ByVal aryParams() As Object)

            Debug.WriteLine("OpenDialogAndWait for '" & strDlgName & "'")

            Dim bDlgUp As Boolean = False
            Dim iCount As Integer = 0
            While Not bDlgUp
                If Not oActionMethod Is Nothing Then
                    'Perform the action method
                    oActionMethod.Invoke(Me, aryParams)
                    Debug.WriteLine("Calling actionmethod '" & oActionMethod.ToString & "'")
                End If

                Threading.Thread.Sleep(1000)

                Dim strFormName As String = DirectCast(ExecuteDirectMethod("ActiveDialogName", Nothing, , True, True), String)
                If strFormName = strDlgName Then
                    bDlgUp = True
                    Debug.WriteLine("Opened '" & strDlgName & "'")
                ElseIf strFormName = "<No Dialog>" Then
                    bDlgUp = False
                    Debug.WriteLine("Dialog was not opened, trying again.")
                Else
                    Dim strErrorText As String
                    If strFormName = "Error" Then
                        strErrorText = CStr(GetApplicationProperty("ErrorDialogMessage"))
                        Throw New System.Exception("The active dialog name does not match the name we are waiting for. Active: '" & strFormName & "', Waiting: '" & strDlgName & "'" & ", Error: " & strErrorText)
                    Else
                        Throw New System.Exception("The active dialog name does not match the name we are waiting for. Active: '" & strFormName & "', Waiting: '" & strDlgName & "'")
                    End If

                End If
                iCount = iCount + 1

                If iCount > 20 Then
                    Throw New System.Exception(strDlgName & " dialog would not open.")
                End If
            End While
        End Sub

        Protected Overridable Sub AssertErrorDialogShown(ByVal strErrorMsg As String, ByVal eMatchType As enumErrorTextType)
            Debug.WriteLine("AssertErrorDialogShown. strErrorMsg:, " & strErrorMsg & ", Math Type: " & eMatchType.ToString)

            OpenDialogAndWait("Error", Nothing, Nothing)
            Threading.Thread.Sleep(1000)
            Dim oVal As Object = GetApplicationProperty("ErrorDialogMessage")
            Threading.Thread.Sleep(1000)
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
            Threading.Thread.Sleep(1000)
            If Not TypeOf oVal Is System.String Then
                Throw New System.Exception("String not returned from error dialog box.")
            End If
            Dim strError As String = CStr(oVal)
            If strError.Trim.Length = 0 Then
                Throw New System.Exception("Error dialog box was not displayed.")
            End If

            Select Case eMatchType
                Case enumErrorTextType.Equals
                    If strError <> strErrorMsg Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumErrorTextType.Contains
                    If Not strError.Contains(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumErrorTextType.BeginsWith
                    If Not strError.StartsWith(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumErrorTextType.EndsWith
                    If Not strError.EndsWith(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case Else
                    Throw New System.Exception("Inavlid match type provided: " & eMatchType.ToString)
            End Select

            Threading.Thread.Sleep(1000)
            Debug.WriteLine("Error dialog shown correctly.")
        End Sub


        Protected Overridable Function ExecuteDirectMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("Executing Direct Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("ExecuteActiveDialogMethod Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteActiveDialogMethod", New Object() {strMethodName, aryParams})
            Threading.Thread.Sleep(iWaitMilliseconds)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteDirectActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("ExecuteDirectActiveDialogMethod Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteDirectActiveDialogMethod", New Object() {strMethodName, aryParams})
            Threading.Thread.Sleep(iWaitMilliseconds)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteIndirectActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False) As Object
            Debug.WriteLine("ExecuteActiveDialogMethod Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteIndirecActiveDialogtMethod", New Object() {strMethodName, aryParams})
            Threading.Thread.Sleep(iWaitMilliseconds)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function GetSimObjectProperty(ByVal strPath As String, ByVal strPropName As String, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("GetSimObjectProperty Path: '" & strPath & "', PropName: '" & strPropName & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("GetObjectProperty", New Object() {strPath, strPropName})
            Threading.Thread.Sleep(iWaitMilliseconds)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function MatchSimObjectPropertyString(ByVal strPath As String, ByVal strPropName As String, ByVal strPropValue As String, Optional ByVal iWaitMilliseconds As Integer = 200) As Boolean
            Dim oPropVal As Object = GetSimObjectProperty(strPath, strPropName, iWaitMilliseconds)
            If oPropVal Is Nothing Then
                Return False
            End If

            If Not TypeOf oPropVal Is String Then
                Return False
            End If

            Dim strVal As String = DirectCast(oPropVal, String)

            If strVal = strPropValue Then
                Return True
            Else
                Return False
            End If
        End Function

        Protected Overridable Function UnitTest(ByVal strAssembly As String, ByVal strClassName As String, ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 0) As Object
            Debug.WriteLine("Calling UnitTest Assembly: '" & strAssembly & "', ClassName: '" & strClassName & "', MethodName: '" & strMethodName & "Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryNewParams As Object() = New Object() {strAssembly, strClassName, strMethodName, aryParams}
            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteObjectMethod", aryNewParams)
            If iWaitMilliseconds > 0 Then
                Threading.Thread.Sleep(iWaitMilliseconds)
            End If

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function GetApplicationProperty(ByVal strPropertyName As String) As Object
            Debug.WriteLine("GetApplicationProperty PropertyName: '" & strPropertyName & "'")

            Dim oRet As Object = m_oServer.GetProperty(strPropertyName)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Sub SetApplicationProperty(ByVal strPropertyName As String, ByVal oData As Object)
            Debug.WriteLine("SetApplicationProperty PropertyName: '" & strPropertyName & "', Value: " & oData.ToString)

            m_oServer.SetProperty(strPropertyName, oData)
        End Sub

        Protected Overridable Sub RunSimulationWaitToEnd()

            If m_bIgnoreSimAndCompare Then Return

            Threading.Thread.Sleep(1000)

            'Start the simulation
            Debug.WriteLine("Running Simulation")
            m_oServer.ExecuteIndirectMethod("ToggleSimulation", Nothing)

            'First wait for it to go into a running state.
            Debug.WriteLine("Waiting for start")
            Dim iIdx As Integer = 0
            Dim bStarted As Boolean = False
            While Not bStarted
                bStarted = DirectCast(m_oServer.GetProperty("SimIsRunning"), Boolean)
                Debug.WriteLine("Checking start: " & bStarted)
                iIdx = iIdx + 1
                If iIdx = 200 Then
                    bStarted = True 'Assume we missed the started command.
                End If
                Threading.Thread.Sleep(20)
            End While

            Debug.WriteLine("Sim Started")
            Threading.Thread.Sleep(1000)

            'Then wait for it to finish a running state.
            Debug.WriteLine("Waiting for Sim end")
            Dim bDone As Boolean = False
            While Not bDone
                Threading.Thread.Sleep(1000)
                CheckForErrorDialog(False)

                bDone = Not DirectCast(m_oServer.GetProperty("SimIsRunning"), Boolean)
                Debug.WriteLine("Checking end: " & bDone)
                iIdx = iIdx + 1
                If iIdx = 200 Then
                    Throw New System.Exception("Timed out waiting for simulation to end.")
                End If
            End While
            Debug.WriteLine("Sim Finished")

        End Sub

        Protected Overridable Sub DeleteDirectory(ByVal strPath As String)
            Dim iAttempts As Integer = 10
            For iTries = 0 To iAttempts
                Try
                    Debug.WriteLine("Attempting to delete directory: " & strPath)
                    System.IO.Directory.Delete(strPath, True)
                Catch ex1 As DirectoryNotFoundException
                    Debug.WriteLine("Directory deleted or does not exist")
                    'The directory has been deleted
                    Return
                Catch ex2 As IOException
                    Debug.WriteLine("Got IOException while attempting to delete dir. Retrying.")
                    Threading.Thread.Sleep(200)
                End Try
            Next
        End Sub

        Public Overridable Sub AssertMatch(ByVal iFound As Integer, ByVal iExpected As Integer, ByVal strName As String)
            Debug.WriteLine("AssertMatch. Found: " & iFound & ", Expected: " & iExpected & ", Name: " & strName)

            If iFound <> iExpected Then
                Throw New System.Exception("Mimatch for variable '" & strName & "'. Expected: " & iExpected & ", Found: " & iFound)
            End If
        End Sub

        Public Overridable Sub ClickMenuItem(ByVal strItemName As String, ByVal bReturnImmediate As Boolean, Optional ByVal iWaitMilliseconds As Integer = 20, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False)
            Debug.WriteLine("ClickMenuItem. Menu Item Name: " & strItemName & ", bReturnImmediate: " & bReturnImmediate)

            ExecuteMethod("ClickMenuItem", New Object() {strItemName, bReturnImmediate}, iWaitMilliseconds, bErrorOk, bSkipWaiting)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

        End Sub

        Public Overridable Overloads Sub ClickToolbarItem(ByVal strItemName As String, ByVal bReturnImmediate As Boolean, Optional ByVal iWaitMilliseconds As Integer = 20, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False)
            Debug.WriteLine("ClickToolbarItem. Toolbar Item Name: " & strItemName & ", bReturnImmediate: " & bReturnImmediate)

            ExecuteMethod("ClickToolbarItem", New Object() {strItemName, bReturnImmediate}, iWaitMilliseconds, bErrorOk, bSkipWaiting)

            WaitWhileBusy(bSkipWaiting, bErrorOk)

        End Sub

        Public Overridable Overloads Sub IndirectClickToolbarItem(ByVal strItemName As String, ByVal bReturnImmediate As Boolean, Optional ByVal iWaitMilliseconds As Integer = 20, Optional ByVal bErrorOk As Boolean = False, Optional ByVal bSkipWaiting As Boolean = False)
            Debug.WriteLine("IndirectClickToolbarItem. Toolbar Item Name: " & strItemName & ", bReturnImmediate: " & bReturnImmediate)

            ExecuteIndirectMethod("ClickToolbarItem", New Object() {strItemName, bReturnImmediate}, iWaitMilliseconds, bErrorOk, bSkipWaiting)

        End Sub

#End Region


    End Class

End Namespace
