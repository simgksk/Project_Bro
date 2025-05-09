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
	using FromProperty = TransformConstraintData.FromProperty;
	using Physics = Skeleton.Physics;
	using ToProperty = TransformConstraintData.ToProperty;

	/// <summary>
	/// <para>
	/// Stores the current pose for a transform constraint. A transform constraint adjusts the world transform of the constrained
	/// bones to match that of the source bone.</para>
	/// <para>
	/// See <a href="http://esotericsoftware.com/spine-transform-constraints">Transform constraints</a> in the Spine User Guide.</para>
	/// </summary>
	public class TransformConstraint : IUpdatable {
		internal readonly TransformConstraintData data;
		internal readonly ExposedList<Bone> bones;
		internal Bone source;
		internal float mixRotate, mixX, mixY, mixScaleX, mixScaleY, mixShearY;

		internal bool active;

		public TransformConstraint (TransformConstraintData data, Skeleton skeleton) {
			if (data == null) throw new ArgumentNullException("data", "data cannot be null.");
			if (skeleton == null) throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			this.data = data;

			bones = new ExposedList<Bone>();
			foreach (BoneData boneData in data.bones)
				bones.Add(skeleton.bones.Items[boneData.index]);

			source = skeleton.bones.Items[data.source.index];

			mixRotate = data.mixRotate;
			mixX = data.mixX;
			mixY = data.mixY;
			mixScaleX = data.mixScaleX;
			mixScaleY = data.mixScaleY;
			mixShearY = data.mixShearY;
		}

		/// <summary>Copy constructor.</summary>
		public TransformConstraint (TransformConstraint constraint, Skeleton skeleton)
			: this(constraint.data, skeleton) {

			mixRotate = constraint.mixRotate;
			mixX = constraint.mixX;
			mixY = constraint.mixY;
			mixScaleX = constraint.mixScaleX;
			mixScaleY = constraint.mixScaleY;
			mixShearY = constraint.mixShearY;
		}

		public void SetToSetupPose () {
			TransformConstraintData data = this.data;
			mixRotate = data.mixRotate;
			mixX = data.mixX;
			mixY = data.mixY;
			mixScaleX = data.mixScaleX;
			mixScaleY = data.mixScaleY;
			mixShearY = data.mixShearY;
		}

		public void Update (Physics physics) {
			if (mixRotate == 0 && mixX == 0 && mixY == 0 && mixScaleX == 0 && mixScaleY == 0 && mixShearY == 0) return;

			TransformConstraintData data = this.data;
			bool localFrom = data.localSource, localTarget = data.localTarget, additive = data.additive, clamp = data.clamp;
			Bone source = this.source;
			FromProperty[] fromItems = data.properties.Items;
			int fn = data.properties.Count;
			Bone[] bones = this.bones.Items;
			for (int i = 0, n = this.bones.Count; i < n; i++) {
				var bone = bones[i];
				for (int f = 0; f < fn; f++) {
					FromProperty from = fromItems[f];
					float value = from.Value(data, source, localFrom) - from.offset;
					ToProperty[] toItems = from.to.Items;
					for (int t = 0, tn = from.to.Count; t < tn; t++) {
						var to = (ToProperty)toItems[t];
						if (to.Mix(this) != 0) {
							float clamped = to.offset + value * to.scale;
							if (clamp) {
								if (to.offset < to.max)
									clamped = MathUtils.Clamp(clamped, to.offset, to.max);
								else
									clamped = MathUtils.Clamp(clamped, to.max, to.offset);
							}
							to.Apply(this, bone, clamped, localTarget, additive);
						}
					}
				}
				if (localTarget)
					bone.Update(Skeleton.Physics.None); // note: reference implementation passes null, ignored parameter
				else
					bone.UpdateAppliedTransform();
			}
		}

		/// <summary>The bones that will be modified by this transform constraint.</summary>
		public ExposedList<Bone> Bones { get { return bones; } }
		/// <summary>The bone whose world transform will be copied to the constrained bones.</summary>
		public Bone Source { get { return source; } set { source = value; } }
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
		public bool Active { get { return active; } }
		/// <summary>The transform constraint's setup pose data.</summary>
		public TransformConstraintData Data { get { return data; } }

		override public string ToString () {
			return data.name;
		}
	}
}
