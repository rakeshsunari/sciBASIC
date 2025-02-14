﻿#Region "Microsoft.VisualBasic::abed2ce454022b1fc8bd07931ab2dc5c, Microsoft.VisualBasic.Core\Extensions\Collection\Linq\Pipeline.vb"

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

    '     Module PipelineExtensions
    ' 
    '         Function: DoCall, PipeOf
    ' 
    '         Sub: DoCall
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace Linq

    <HideModuleName> Public Module PipelineExtensions

        ''' <summary>
        ''' Delegate pipeline function
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <typeparam name="Tout"></typeparam>
        ''' <param name="input"></param>
        ''' <param name="apply"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function DoCall(Of T, Tout)(input As T, apply As Func(Of T, Tout)) As Tout
            Return apply(input)
        End Function

        ''' <summary>
        ''' Delegate pipeline function
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="input"></param>
        ''' <param name="apply"></param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Sub DoCall(Of T)(input As T, apply As Action(Of T))
            Call apply(input)
        End Sub

        <Extension>
        Public Function PipeOf(Of T, Rest)(input As T, task As Action(Of T, Rest)) As Action(Of Rest)
            Return Sub(a) task(input, a)
        End Function
    End Module
End Namespace
