﻿#Region "Microsoft.VisualBasic::c75882c2ae2d3f489422ea4953d52775, Microsoft.VisualBasic.Core\ApplicationServices\Tools\Zip\UnZip.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Module UnZip
    ' 
    '         Function: ExtractToSelfDirectory
    ' 
    '         Sub: ExtractToFileInternal, ImprovedExtractToDirectory, ImprovedExtractToFile
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace ApplicationServices.Zip

    Public Module UnZip

        <Extension>
        Private Sub ExtractToFileInternal(file As ZipArchiveEntry, destinationPath$, overwriteMethod As Overwrite, overridesFullName$)
            ' Gets the complete path for the destination file, including any
            ' relative paths that were in the zip file
            Dim destinationFileName As String = Path.Combine(destinationPath, overridesFullName Or file.FullName.AsDefault)

            ' Gets just the new path, minus the file name so we can create the
            ' directory if it does not exist
            Dim destinationFilePath As String = Path.GetDirectoryName(destinationFileName)

            ' Creates the directory (if it doesn't exist) for the new path
            ' 2018-2-2 在原先的代码之中直接使用CreateDirectory，如果目标文件夹存在的话会报错
            ' 在这里使用安全一点的mkdir函数
            Call destinationFilePath.MkDIR(throwEx:=False)

            ' Determines what to do with the file based upon the
            ' method of overwriting chosen
            Select Case overwriteMethod
                Case Overwrite.Always

                    ' Just put the file in and overwrite anything that is found
                    file.ExtractToFile(destinationFileName, True)

                Case Overwrite.IfNewer
                    ' Checks to see if the file exists, and if so, if it should
                    ' be overwritten
                    If Not IO.File.Exists(destinationFileName) OrElse IO.File.GetLastWriteTime(destinationFileName) < file.LastWriteTime Then
                        ' Either the file didn't exist or this file is newer, so
                        ' we will extract it and overwrite any existing file
                        file.ExtractToFile(destinationFileName, True)
                    End If

                Case Overwrite.Never
                    ' Put the file in if it is new but ignores the 
                    ' file if it already exists
                    If Not IO.File.Exists(destinationFileName) Then
                        file.ExtractToFile(destinationFileName)
                    End If

                Case Else
            End Select
        End Sub

        ''' <summary>
        ''' Safely extracts a single file from a zip file
        ''' </summary>
        ''' <param name="file">
        ''' The zip entry we are pulling the file from
        ''' </param>
        ''' <param name="destinationPath">
        ''' The root of where the file is going
        ''' </param>
        ''' <param name="overwriteMethod">
        ''' Specifies how we are going to handle an existing file.
        ''' The default is Overwrite.IfNewer.
        ''' </param>
        ''' 
        <ExportAPI("Extract", Info:="Safely extracts a single file from a zip file.")>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension> Public Sub ImprovedExtractToFile(<Parameter("Zip.Entry", "The zip entry we are pulling the file from")>
                                                     file As ZipArchiveEntry,
                                                     destinationPath$,
                                                     Optional overwriteMethod As Overwrite = Overwrite.IfNewer)
            Call file.ExtractToFileInternal(destinationPath, overwriteMethod, Nothing)
        End Sub

        Public Function ExtractToSelfDirectory(zip$, Optional overwriteMethod As Overwrite = Overwrite.IfNewer) As String
            Dim Dir As String = FileIO.FileSystem.GetParentPath(zip)
            Dim Name As String = BaseName(zip)

            Dir = Dir & "/" & Name

            Call ImprovedExtractToDirectory(zip, Dir, overwriteMethod)

            Return Dir
        End Function

        ''' <summary>
        ''' Unzips the specified file to the given folder in a safe
        ''' manner.  This plans for missing paths and existing files
        ''' and handles them gracefully.
        ''' </summary>
        ''' <param name="sourceArchiveFileName">
        ''' The name of the zip file to be extracted
        ''' </param>
        ''' <param name="destinationDirectoryName">
        ''' The directory to extract the zip file to
        ''' </param>
        ''' <param name="overwriteMethod">
        ''' Specifies how we are going to handle an existing file.
        ''' The default is IfNewer.
        ''' </param>
        ''' 
        <ExportAPI("ExtractToDir", Info:="Unzips the specified file to the given folder in a safe manner. This plans for missing paths and existing files and handles them gracefully.")>
        Public Sub ImprovedExtractToDirectory(<Parameter("Zip", "The name of the zip file to be extracted")> sourceArchiveFileName$,
                                              <Parameter("Dir", "The directory to extract the zip file to")> destinationDirectoryName$,
                                              <Parameter("Overwrite.HowTo", "Specifies how we are going to handle an existing file. The default is IfNewer.")>
                                              Optional overwriteMethod As Overwrite = Overwrite.IfNewer,
                                              Optional extractToFlat As Boolean = False)

            ' Opens the zip file up to be read
            Using archive As ZipArchive = ZipFile.OpenRead(sourceArchiveFileName)

                Dim rootDir$ = Nothing
                Dim isFolderArchive = sourceArchiveFileName.IsSourceFolderZip(folder:=rootDir)
                Dim fullName$

                ' Loops through each file in the zip file
                For Each file As ZipArchiveEntry In archive.Entries
                    If extractToFlat AndAlso isFolderArchive Then
                        fullName = file.FullName.Replace(rootDir, "")
                    Else
                        fullName = file.FullName
                    End If

                    If file.IsADirectoryEntry Then
                        Call Path _
                            .Combine(destinationDirectoryName, fullName) _
                            .MkDIR
                    Else
                        Call file.ExtractToFileInternal(
                            destinationPath:=destinationDirectoryName,
                            overwriteMethod:=overwriteMethod,
                            overridesFullName:=fullName
                        )
                    End If
                Next
            End Using
        End Sub
    End Module
End Namespace
