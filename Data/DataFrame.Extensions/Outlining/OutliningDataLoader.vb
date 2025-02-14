﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.ComponentModels
Imports TableSchema = Microsoft.VisualBasic.Data.csv.StorageProvider.ComponentModels.SchemaProvider

Namespace Outlining

    Public Module OutliningDataLoader

        <Extension>
        Friend Function createBuilderByHeaders(type As Type, headers As IEnumerable(Of String), strict As Boolean) As RowBuilder
            Dim schema As TableSchema = TableSchema.CreateObject(type, strict).CopyWriteDataToObject
            Dim rowBuilder As New RowBuilder(schema)

            Call rowBuilder.IndexOf(New HeaderSchema(headers))
            Call rowBuilder.SolveReadOnlyMetaConflicts()

            Return rowBuilder
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="filepath"></param>
        ''' <param name="ignoresBlankRow">
        ''' 如果遇到空白行是抛出错误还是忽略掉该空白行？默认是不忽略掉该空白行，即抛出错误
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Public Iterator Function LoadOutlining(Of T As Class)(filepath$,
                                                              Optional strict As Boolean = False,
                                                              Optional ignoresBlankRow As Boolean = True,
                                                              Optional metaBlank$ = Nothing) As IEnumerable(Of T)
            Dim file As File = File.Load(filepath)
            ' 按照列空格进行文件的等级切割
            Dim indent As Integer
            Dim currentIndent As Integer = -1
            Dim builder As New Builder(GetType(T), file.Headers, strict)
            Dim indentBuilderCache As Builder = Nothing
            Dim obj As T = Nothing

            For Each row As RowObject In file.Skip(1)
                indent = row.RowIndentLevel

                If indent < 0 Then
                    If ignoresBlankRow Then
                        Continue For
                    Else
                        Throw New DataException($"Row blank!")
                    End If
                Else
                    If currentIndent <> indent Then
                        currentIndent = indent

                        If currentIndent = 0 Then
                            If Not obj Is Nothing Then
                                Yield builder.Flush(obj)
                            End If

                            obj = Activator.CreateInstance(GetType(T))
                            obj = builder.Builder.FillData(row, obj, metaBlank)
                        Else
                            ' is subtable row header
                            indentBuilderCache = builder.GetBuilder(currentIndent)

                            If indentBuilderCache.Builder Is Nothing Then
                                Call builder.CreateBuilder(indent, row, strict)
                            End If
                        End If
                    Else
                        Call indentBuilderCache.CacheObject(row, metaBlank)
                    End If
                End If
            Next

            If Not obj Is Nothing Then
                Yield builder.Flush(obj)
            End If
        End Function

        <Extension>
        Public Function RowIndentLevel(row As RowObject) As Integer
            For i As Integer = 0 To row.NumbersOfColumn - 1
                If Not row(i).StringEmpty(whitespaceAsEmpty:=False) Then
                    Return i
                End If
            Next

            ' 这个是一个空行？？
            Return -1
        End Function
    End Module
End Namespace
