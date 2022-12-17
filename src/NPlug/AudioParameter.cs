// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace NPlug;

public class AudioParameter
{
    /// <summary>
    /// This value is only used temporarily until the <see cref="AudioProcessorModel"/> is initialized.
    /// Then it is the <see cref="PointerToNormalizedValueInSharedBuffer"/> that is used.
    /// </summary>
    internal double LocalNormalizedValue;

    /// <summary>
    /// This pointer is setup by <see cref="AudioProcessorModel"/> in InitializeBuffers.
    /// </summary>
    internal unsafe double* PointerToNormalizedValueInSharedBuffer;

    protected AudioParameterInfo InfoBase;

    public AudioParameter(AudioParameterInfo info)
    {
        Id = info.Id;
        InfoBase = info;
        Precision = 4;
        NormalizedValue = Info.DefaultNormalizedValue;
    }

    public AudioParameter(string title, int id = 0, string? units = null, string? shortTitle = null, int stepCount = 0, double defaultNormalizedValue = 0.0, AudioParameterFlags flags = AudioParameterFlags.CanAutomate)
    {
        Id = id;
        InfoBase = new AudioParameterInfo(id, title)
        {
            Units = units ?? string.Empty,
            ShortTitle = shortTitle ?? title,
            StepCount = stepCount,
            DefaultNormalizedValue = defaultNormalizedValue,
            Flags = flags
        };
        Precision = 4;
        NormalizedValue = defaultNormalizedValue;
    }

    public bool IsControllerOnly { get; init; }

    public AudioParameterId Id { get; internal set; }

    public AudioParameterInfo Info => InfoBase with
    {
        Id = Id,
        UnitId = Unit?.Id ?? AudioUnitId.NoParent
    };
    
    public AudioUnit? Unit { get; internal set; }

    /// <summary>
    /// Gets a direct access to the raw normalized value.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="NormalizedValue"/>, this doesn't clamp or trigger a <see cref="Changed"/> event.
    /// </remarks>
    internal unsafe ref double RawNormalizedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (PointerToNormalizedValueInSharedBuffer != null)
            { 
                return ref *PointerToNormalizedValueInSharedBuffer;
            }
            else
            {
                return ref LocalNormalizedValue;
            }
        }
    }

    public double NormalizedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => RawNormalizedValue;
        set
        {
            value = Math.Clamp(value, 0.0, 1.0);
            ref var rawNormalizedValue = ref RawNormalizedValue;
            if (rawNormalizedValue != value)
            {
                rawNormalizedValue = value;
                OnParameterValueChanged();
            }
        }
    }

    public int Precision { get; set; }

    /// <summary>
    /// Convert a normalized value to a plain value as a string.
    /// </summary>
    /// <param name="valueNormalized"></param>
    /// <returns></returns>
    public virtual string ToString(double valueNormalized)
    {
        if (Info.StepCount == 1)
        {
            return RawNormalizedValue > 0.5 ? "On" : "Off";
        }

        return valueNormalized.ToString(GetPrecisionFormat(Precision));
    }

    /// <summary>
    /// Convert a plain value as a string to a normalized value.
    /// </summary>
    /// <param name="plainValueAsString"></param>
    /// <returns></returns>
    public virtual double FromString(string plainValueAsString)
    {
        if (Info.StepCount == 1)
        {
            return (plainValueAsString.Equals("on", StringComparison.OrdinalIgnoreCase) || plainValueAsString.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1.0 : 0.0;
        }
        double.TryParse(plainValueAsString, out var value);
        return value;
    }

    public virtual double ToPlain(double normalizedValue)
    {
        return normalizedValue;
    }

    public virtual double ToNormalized(double plainValue)
    {
        return plainValue;
    }

    public override string ToString()
    {
        return $"[{Info.Id.Value}] {Info.Title} = {ToString(NormalizedValue)}";
    }

    private void OnParameterValueChanged()
    {
        Unit?.OnParameterValueChangedInternal(this);
    }

    private static string GetPrecisionFormat(int precision)
    {
        return precision switch
        {
            0 => "N0",
            1 => "N1",
            2 => "N2",
            3 => "N3",
            4 => "N4",
            5 => "N5",
            6 => "N6",
            7 => "N7",
            8 => "N8",
            9 => "N9",
            10 => "N10",
            11 => "N11",
            12 => "N12",
            13 => "N13",
            14 => "N14",
            15 => "N15",
            16 => "N16",
            _ => "N4",
        };
    }
}