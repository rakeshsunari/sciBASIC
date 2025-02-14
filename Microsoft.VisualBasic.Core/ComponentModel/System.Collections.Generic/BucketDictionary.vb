﻿#Region "Microsoft.VisualBasic::fb63cde407f533b915185b636ca705d8, Microsoft.VisualBasic.Core\ComponentModel\System.Collections.Generic\BucketDictionary.vb"

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

    '     Class BucketDictionary
    ' 
    '         Properties: Count, Keys, Values
    ' 
    '         Constructor: (+3 Overloads) Sub New
    '         Function: ContainsKey, GetEnumerator, IEnumerable_GetEnumerator, ToString, TryGetValue
    ' 
    '     Module BucketDictionaryExtensions
    ' 
    '         Function: (+3 Overloads) CreateBuckets
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ComponentModel.Collection

    ''' <summary>
    ''' An ultralarge size dictionary object.
    ''' (当你发现一个数据集合非常的大的时候，一个字典会出现溢出，则这个时候就需要这个超大容量的Bucket字典容器了)
    ''' </summary>
    ''' <typeparam name="K"></typeparam>
    ''' <typeparam name="V"></typeparam>
    Public Class BucketDictionary(Of K, V) : Implements IReadOnlyDictionary(Of K, V)

        Friend ReadOnly buckets As New List(Of Dictionary(Of K, V))
        Friend minBucket As Dictionary(Of K, V)

        ''' <summary>
        ''' 每一个字典之中的最大的元素数目
        ''' </summary>
        ReadOnly bucketSize As Integer

        ''' <summary>
        ''' 获取这个超大的字典集合之中的对象的数量总数
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Count As Integer Implements IReadOnlyCollection(Of KeyValuePair(Of K, V)).Count
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return buckets.Sum(Function(x) x.Count)
            End Get
        End Property

        ''' <summary>
        ''' 注意，不要直接使用这个方法来添加新的数据，使用<see cref="BucketDictionaryExtensions"/>的方法会更加高效
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        Default Public Property Item(key As K) As V Implements IReadOnlyDictionary(Of K, V).Item
            Get
                For Each hash In buckets
                    If hash.ContainsKey(key) Then
                        Return hash(key)
                    End If
                Next

                Return Nothing
            End Get
            Set(value As V)
                If buckets.Count = 0 Then
                    buckets.Add(New Dictionary(Of K, V) From {{key, value}})
                    minBucket = buckets(Scan0)
                Else
                    For Each hash In buckets
                        If hash.ContainsKey(key) Then
                            hash(key) = value
                            Return
                        End If
                    Next

                    minBucket(key) = value

                    If minBucket.Count >= bucketSize Then
                        buckets.Add(New Dictionary(Of K, V))
                        minBucket = buckets.Last
                    End If
                End If
            End Set
        End Property

        Public ReadOnly Property Keys As IEnumerable(Of K) Implements IReadOnlyDictionary(Of K, V).Keys
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return buckets.Select(Function(x) x.Keys).IteratesALL
            End Get
        End Property

        Public ReadOnly Property Values As IEnumerable(Of V) Implements IReadOnlyDictionary(Of K, V).Values
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return buckets.Select(Function(x) x.Values).IteratesALL
            End Get
        End Property

        Sub New(bucketSize As Integer)
            Me.bucketSize = bucketSize
        End Sub

        Sub New()
            Call Me.New(Short.MaxValue * 10)
        End Sub

        Sub New(buckets As IEnumerable(Of Dictionary(Of K, V)))
            Me.buckets = buckets.AsList
            Me.minBucket = Me.buckets _
                .OrderBy(Function(table) table.Count) _
                .FirstOrDefault
        End Sub

        Public Overrides Function ToString() As String
            Return $"Tuple of [{GetType(K).Name}, {GetType(V).Name}] with {Count} records in {buckets.Count} buckets."
        End Function

        Public Function ContainsKey(key As K) As Boolean Implements IReadOnlyDictionary(Of K, V).ContainsKey
            For Each hash In buckets
                If hash.ContainsKey(key) Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Iterator Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of K, V)) Implements IEnumerable(Of KeyValuePair(Of K, V)).GetEnumerator
            For Each hash In buckets
                For Each x In hash
                    Yield x
                Next
            Next
        End Function

        Public Function TryGetValue(key As K, ByRef value As V) As Boolean Implements IReadOnlyDictionary(Of K, V).TryGetValue
            For Each hash In buckets
                If hash.ContainsKey(key) Then
                    value = hash(key)
                    Return True
                End If
            Next

            Return False
        End Function

        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield GetEnumerator()
        End Function
    End Class

    <HideModuleName>
    Public Module BucketDictionaryExtensions

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <typeparam name="K"></typeparam>
        ''' <param name="source"></param>
        ''' <param name="getKey"></param>
        ''' <param name="size%"></param>
        ''' <param name="overridesDuplicates">
        ''' 当数据中存在重复的键名的时候，是将前面已有的数据覆盖掉还是抛出键名重复的错误？默认是不进行重写覆盖，而是键重复抛出错误。
        ''' </param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function CreateBuckets(Of T, K)(source As IEnumerable(Of T),
                                               getKey As Func(Of T, K),
                                               Optional size% = Short.MaxValue * 10,
                                               Optional overridesDuplicates As Boolean = False) As BucketDictionary(Of K, T)
            Return source.CreateBuckets(
                getKey:=getKey,
                getValue:=Function(o) o,
                size:=size,
                overridesDuplicates:=overridesDuplicates
            )
        End Function

        <Extension>
        Public Function CreateBuckets(Of T, K, V)(source As IEnumerable(Of T),
                                                  getKey As Func(Of T, K),
                                                  getValue As Func(Of T, V),
                                                  Optional size% = Short.MaxValue * 10,
                                                  Optional overridesDuplicates As Boolean = False) As BucketDictionary(Of K, V)

            Dim table As New BucketDictionary(Of K, V)(size)
            Dim bucket As New Dictionary(Of K, V)

            For Each x As T In source
                Dim key As K = getKey(x)
                Dim value As V = getValue(x)

                If overridesDuplicates Then
                    bucket(key) = value
                ElseIf bucket.ContainsKey(key) Then
                    Throw New DuplicateNameException(key.GetJson)
                Else
                    Call bucket.Add(key, value)
                End If

                If bucket.Count >= size Then
                    table.buckets.Add(bucket)
                    bucket = New Dictionary(Of K, V)
                End If
            Next

            Call table.buckets.Add(bucket)

            Return table
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function CreateBuckets(Of K, V)(source As IEnumerable(Of (K, V)), Optional size% = Short.MaxValue * 10) As BucketDictionary(Of K, V)
            Return source.CreateBuckets(Function(t) t.Item1, Function(t) t.Item2, size:=size)
        End Function
    End Module
End Namespace
