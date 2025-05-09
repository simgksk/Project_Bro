/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated April 5, 2025. Replaces all prior versions.
 *
 * Copyright (c) 2013-2025, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System;

namespace Spine {
	public class TransformConstraintData : ConstraintData {
		internal readonly ExposedList<BoneData> bones = new ExposedList<BoneData>();
		internal BoneData source;
		internal float mixRotate, mixX, mixY, mixScaleX, mixScaleY, mixShearY;
		internal float offsetRotation, offsetX, offsetY, offsetScaleX, offsetScaleY, offsetShearY;
		internal bool localSource, localTarget, additive, clamp;
		internal readonly ExposedList<FromProperty> properties = new ExposedList<FromProperty>();

		public TransformConstraintData (string name) : base(name) {
		}

		public ExposedList<BoneData> Bones { get { return bones; } }

		/// <summary>The bone whose world transform will be copied to the constrained bones.</summary>
		public BoneData Source {
			get { return source; }
			set {
				if (source == null) throw new ArgumentNullException("Source", "source cannot be null.");
				source = value;
			}
		}

		/// <summary>The mapping of transform properties to other transform properties.</summary>
		public ExposedList<FromProperty> Properties {
			get { return properties; }
		}

		/// <summary>A percentage (0-1) that controls the mix between the constrained and unconstrained rotation.</summary>
		public float MixRotate { get { return mixRotate; } set { mixRotate = value; } }
		/// <summary>A percentage (0-1) that controls the mix between the constrained and unconstrained translation X.</summary>
		public float MixX { get { return mixX; } set { mixX = value; } }
		/// <summary>A percentage (0-1) that controls the mix between the constrained and unconstrained translation Y.</summary>
		public float MixY { get { return mixY; } set { mixY = value; } }
		/// <summary>A percentage (0-1) that controls the mix between the constrained and unconstrained scale X.</summary>
		public float MixScaleX { get { return mixScaleX; } set { mixScaleX = value; } }
		/// <summary>A percentage (0-1) that controls the mix between the constrained and unconstrained scale Y.</summary>
		public float MixScaleY { get { return mixScaleY; } set { mixScaleY = value; } }
		/// <summary>A percentage (0-1) that controls the mix between the constrained and unconstrained shear Y.</summary>
		public float MixShearY { get { return mixShearY; } set { mixShearY = value; } }
		/// <summary>An offset added to the constrained bone rotation.</summary>
		public float OffsetRotation { get { return offsetRotation; } set { offsetRotation = value; } }
		/// <summary>An offset added to the constrained bone X translation.</summary>
		public float OffsetX { get { return offsetX; } set { offsetX = value; } }
		/// <summary>An offset added to the constrained bone Y translation.</summary>
		public float OffsetY { get { return offsetY; } set { offsetY = value; } }
		/// <summary>An offset added to the constrained bone scaleX.</summary>
		public float OffsetScaleX { get { return offsetScaleX; } set { offsetScaleX = value; } }
		/// <summary>An offset added to the constrained bone scaleY.</summary>
		public float OffsetScaleY { get { return offsetScaleY; } set { offsetScaleY = value; } }
		/// <summary>An offset added to the constrained bone shearY.</summary>
		public float OffsetShearY { get { return offsetShearY; } set { offsetShearY = value; } }

		/// <summary>Reads the source bone's local transform instead of its world transform.</summary>
		public bool LocalSource { get { return localSource; } set { localSource = value; } }
		/// <summary>Sets the constrained bones' local transforms instead of their world transforms.</summary>
		public bool LocalTarget { get { return localTarget; } set { localTarget = value; } }
		/// <summary>Adds the source bone transform to the constrained bones instead of setting it absolutely.</summary>
		public bool Additive { get { return additive; } set { additive = value; } }
		/// <summary>Prevents constrained bones from exceeding the ranged defined by <see cref="ToProperty.offset"/> and
		/// <see cref="ToProperty.max"/>.</summary>
		public bool Clamp { get { return clamp; } set { clamp = value; } }

		/// <summary>Source property for a <see cref="TransformConstraint"/>.</summary>
		abstract public class FromProperty {
			/// <summary>The value of this property that corresponds to <see cref="ToProperty.offset"/>.</summary>
			public float offset;

			/// <summary>Constrained properties.</summary>
			public readonly ExposedList<ToProperty> to = new ExposedList<ToProperty>();

			/// <summary>Reads this property from the specified bone.</summary>
			abstract public float Value (TransformConstraintData data, Bone source, bool local);
		}

		///<summary>Constrained property for a <see cref="TransformConstraint"/>.</summary>
		abstract public class ToProperty {
			/// <summary>The value of this property that corresponds to <see cref="FromProperty.offset"/>.</summary>
			public float offset;

			/// <summary>The maximum value of this property when <see cref="TransformConstraintData.clamp"/> clamped.</summary>
			public float max;

			/// <summary>The scale of the <see cref="FromProperty"/> value in relation to this property.</summary>
			public float scale;

			/// <summary>Reads the mix for this property from the specified constraint.</summary>
			public abstract float Mix (TransformConstraint constraint);

			/// <summary>Applies the value to this property.</summary>
			public abstract void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive);
		}

		public class FromRotate : FromProperty {
			public override float Value (TransformConstraintData data, Bone source, bool local) {
				if (local) return source.arotation + data.offsetRotation;
				float value = MathUtils.Atan2(source.c, source.a) * MathUtils.RadDeg
					+ (source.a * source.d - source.b * source.c > 0 ? data.offsetRotation : -data.offsetRotation);
				if (value < 0) value += 360;
				return value;
			}
		}

		public class ToRotate : ToProperty {
			public override float Mix (TransformConstraint constraint) {
				return constraint.mixRotate;
			}

			public override void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive) {
				if (local) {
					if (!additive) value -= bone.arotation;
					bone.arotation += value * constraint.mixRotate;
				} else {
					float a = bone.a, b = bone.b, c = bone.c, d = bone.d;
					value *= MathUtils.DegRad;
					if (!additive) value -= MathUtils.Atan2(c, a);
					if (value > MathUtils.PI)
						value -= MathUtils.PI2;
					else if (value < -MathUtils.PI) //
						value += MathUtils.PI2;
					value *= constraint.mixRotate;
					float cos = MathUtils.Cos(value), sin = MathUtils.Sin(value);
					bone.a = cos * a - sin * c;
					bone.b = cos * b - sin * d;
					bone.c = sin * a + cos * c;
					bone.d = sin * b + cos * d;
				}
			}
		}

		public class FromX : FromProperty {
			public override float Value (TransformConstraintData data, Bone source, bool local) {
				return local ? source.ax + data.offsetX : data.offsetX * source.a + data.offsetY * source.b + source.worldX;
			}
		}

		public class ToX : ToProperty {
			public override float Mix (TransformConstraint constraint) {
				return constraint.mixX;
			}

			public override void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive) {
				if (local) {
					if (!additive) value -= bone.ax;
					bone.ax += value * constraint.mixX;
				} else {
					if (!additive) value -= bone.worldX;
					bone.worldX += value * constraint.mixX;
				}
			}
		}

		public class FromY : FromProperty {
			public override float Value (TransformConstraintData data, Bone source, bool local) {
				return local ? source.ay + data.offsetY : data.offsetX * source.c + data.offsetY * source.d + source.worldY;
			}
		}

		public class ToY : ToProperty {
			public override float Mix (TransformConstraint constraint) {
				return constraint.mixY;
			}

			public override void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive) {
				if (local) {
					if (!additive) value -= bone.ay;
					bone.ay += value * constraint.mixY;
				} else {
					if (!additive) value -= bone.worldY;
					bone.worldY += value * constraint.mixY;
				}
			}
		}

		public class FromScaleX : FromProperty {
			public override float Value (TransformConstraintData data, Bone source, bool local) {
				return (local ? source.ascaleX : (float)Math.Sqrt(source.a * source.a + source.c * source.c)) + data.offsetScaleX;
			}
		}

		public class ToScaleX : ToProperty {
			public override float Mix (TransformConstraint constraint) {
				return constraint.mixScaleX;
			}

			public override void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive) {
				if (local) {
					if (additive)
						bone.ascaleX *= 1 + ((value - 1) * constraint.mixScaleX);
					else if (bone.ascaleX != 0) //
						bone.ascaleX = 1 + (value / bone.ascaleX - 1) * constraint.mixScaleX;
				} else {
					float s;
					if (additive)
						s = 1 + (value - 1) * constraint.mixScaleX;
					else {
						s = (float)Math.Sqrt(bone.a * bone.a + bone.c * bone.c);
						if (s != 0) s = 1 + (value / s - 1) * constraint.mixScaleX;
					}
					bone.a *= s;
					bone.c *= s;
				}
			}
		}

		public class FromScaleY : FromProperty {
			public override float Value (TransformConstraintData data, Bone source, bool local) {
				return (local ? source.ascaleY : (float)Math.Sqrt(source.b * source.b + source.d * source.d)) + data.offsetScaleY;
			}
		}

		public class ToScaleY : ToProperty {
			public override float Mix (TransformConstraint constraint) {
				return constraint.mixScaleY;
			}

			public override void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive) {
				if (local) {
					if (additive)
						bone.ascaleY *= 1 + ((value - 1) * constraint.mixScaleY);
					else if (bone.ascaleY != 0) //
						bone.ascaleY = 1 + (value / bone.ascaleY - 1) * constraint.mixScaleY;
				} else {
					float s;
					if (additive)
						s = 1 + (value - 1) * constraint.mixScaleY;
					else {
						s = (float)Math.Sqrt(bone.b * bone.b + bone.d * bone.d);
						if (s != 0) s = 1 + (value / s - 1) * constraint.mixScaleY;
					}
					bone.b *= s;
					bone.d *= s;
				}
			}
		}

		public class FromShearY : FromProperty {
			public override float Value (TransformConstraintData data, Bone source, bool local) {
				return (local ? source.ashearY : (MathUtils.Atan2(source.d, source.b) - MathUtils.Atan2(source.c, source.a)) * MathUtils.RadDeg - 90)
					+ data.offsetShearY;
			}
		}

		public class ToShearY : ToProperty {
			public override float Mix (TransformConstraint constraint) {
				return constraint.mixShearY;
			}

			public override void Apply (TransformConstraint constraint, Bone bone, float value, bool local, bool additive) {
				if (local) {
					if (!additive) value -= bone.ashearY;
					bone.ashearY += value * constraint.mixShearY;
				} else {
					float b = bone.b, d = bone.d, by = MathUtils.Atan2(d, b);
					value = (value + 90) * MathUtils.DegRad;
					if (additive)
						value -= MathUtils.PI / 2;
					else {
						value -= by - MathUtils.Atan2(bone.c, bone.a);
						if (value > MathUtils.PI)
							value -= MathUtils.PI2;
						else if (value < -MathUtils.PI) //
							value += MathUtils.PI2;
					}
					value = by + value * constraint.mixShearY;
					float s = (float)Math.Sqrt(b * b + d * d);
					bone.b = MathUtils.Cos(value) * s;
					bone.d = MathUtils.Sin(value) * s;
				}
			}
		}
	}
}
