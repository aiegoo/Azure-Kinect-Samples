using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace com.rfilkov.kinect
{
	/// <summary>
	/// KinectManager is the the main and most basic depth-sensor related component. It controls the sensors and manages the data streams.
	/// </summary>
	public partial class KinectManager : MonoBehaviour
	{
		public ulong FixedUserID = 0;

		protected Texture2D usersLblTex2D = null;
		public float kinect4AzureFactor = -1f;

		private void Start()
		{
			if (IsKinect4Azure())
			{
				kinect4AzureFactor = -1f;
			}
			else if (IsKinectV2())
			{
				kinect4AzureFactor = 1f;
			}
		}

		/// <param name="userId">User ID</param>
		/// <param name="joint">Joint index</param>
		public Vector3 GetJointKinectPosition(ulong userId, int joint)
		{
			if (userManager.dictUserIdToIndex.ContainsKey(userId))
			{
				int index = userManager.dictUserIdToIndex[userId];

				if (index >= 0 && index < trackedBodiesCount && alTrackedBodies[index].bIsTracked)
				{
					if (joint >= 0 && joint < (int)KinectInterop.JointType.Count)
					{
						KinectInterop.JointData jointData = alTrackedBodies[index].joint[joint];

						return ApplyAzureFactor(jointData.kinectPos);
					}
				}
			}

			return Vector3.zero;
		}

		public Vector3 GetJointKinectOrignalPosition(ulong userId, int joint)
		{
			if (userManager.dictUserIdToIndex.ContainsKey(userId))
			{
				
				int index = userManager.dictUserIdToIndex[userId];

				if (index >= 0 && index < trackedBodiesCount && alTrackedBodies[index].bIsTracked)
				{
					if (joint >= 0 && joint < (int)KinectInterop.JointType.Count)
					{
						KinectInterop.JointData jointData = alTrackedBodies[index].joint[joint];

						return jointData.kinectPos;
					}
				}
			}

			return Vector3.zero;
		}

		public bool IsJointKinectTracked(ulong userId, int joint)
		{
			int index = 0;

			try
            {
				index = userManager.dictUserIdToIndex[userId];
			}
			catch(Exception ex)
            {

            }

			if (index >= 0 &&
					index < trackedBodiesCount &&
					alTrackedBodies[index].bIsTracked &&
					alTrackedBodies[index].joint[joint].trackingState == KinectInterop.TrackingState.Tracked
					)
			{
				return true;
			}

			return false;
		}

		/// <returns>The sensor data.</returns>
		internal KinectInterop.SensorData GetSensorData()
		{
			return GetSensorData(0);
		}

		/// <summary>
		/// Gets the users 2d-texture, if ComputeUserMap is true
		/// </summary>
		/// <returns>The users 2d-texture.</returns>
		public Texture GetUsersLblTex()
		{

			int si = 0;  // sensor 0
			if (si >= 0 && si < sensorDatas.Count)
			{
				KinectInterop.SensorData sensorData = sensorDatas[si];
				return sensorData.bodyImageTexture;
			}

			return null;
		}

		/// <summary>
		/// Gets the users 2d-texture, if ComputeUserMap is true
		/// </summary>
		/// <returns>The users 2d-texture.</returns>
		public Texture2D GetUsersLblTex2D()
		{
			int si = 0;  // sensor 0
			KinectInterop.SensorData sensorData;
			if (si >= 0 && si < sensorDatas.Count)
			{
				sensorData = sensorDatas[si];
			}
			else
			{
				return null;
			}

			if (usersLblTex2D == null)
			{
				usersLblTex2D = new Texture2D(sensorData.depthImageWidth, sensorData.depthImageHeight, TextureFormat.ARGB32, false);
			}

			if (sensorData.bodyImageTexture)
			{
				KinectInterop.RenderTex2Tex2D(sensorData.bodyImageTexture, ref usersLblTex2D);
			}

			return usersLblTex2D;
		}

		public void SetDisplayUserMap(bool view)
		{
			displayImages.Clear();

			if (view)
			{
				displayImages.Add(DisplayImageType.UserBodyImage);
			}
		}

		public bool ViewDisplayUserMap()
		{
			return displayImages.Count > 0;
		}

		public bool IsKinect4Azure()
		{
			KinectInterop.SensorData sensorData = GetSensorData(0);

			if (sensorData != null)
			{
				if (sensorData.sensorInterface.GetSensorPlatform() == KinectInterop.DepthSensorPlatform.Kinect4Azure)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsKinectV2()
		{
			KinectInterop.SensorData sensorData = GetSensorData(0);

			if (sensorData != null)
			{
				if (sensorData.sensorInterface.GetSensorPlatform() == KinectInterop.DepthSensorPlatform.KinectV2)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the 3d overlay position of the given joint over the depth-image.
		/// </summary>
		/// <returns>The joint position for depth overlay.</returns>
		/// <param name="userId">User ID</param>
		/// <param name="joint">Joint index</param>
		/// <param name="imageRect">Depth image rectangle on the screen</param>
		public Vector2 GetJointPosDepthOverlay(ulong userId, int joint, Rect imageRect)
		{
			if (userManager.dictUserIdToIndex.ContainsKey(userId))
			{
				int index = userManager.dictUserIdToIndex[userId];
				int sensorIndex = GetPrimaryBodySensorIndex();

				if (index >= 0 && index < trackedBodiesCount && alTrackedBodies[index].bIsTracked)
				{
					if (joint >= 0 && joint < (int)KinectInterop.JointType.Count)
					{
						KinectInterop.JointData jointData = alTrackedBodies[index].joint[joint];
						Vector3 posJointRaw = jointData.kinectPos;
						posJointRaw.x = posJointRaw.x * kinect4AzureFactor;

						if (posJointRaw != Vector3.zero)
						{
							// 3d position to depth
							Vector2 posDepth = MapSpacePointToDepthCoords(sensorIndex, posJointRaw);

							if (posDepth != Vector2.zero)
							{
								if (!float.IsInfinity(posDepth.x) && !float.IsInfinity(posDepth.y))
								{
									KinectInterop.SensorData sensorData = GetSensorData(sensorIndex);

									float xScaled = (float)posDepth.x * imageRect.width / sensorData.depthImageWidth;
									float yScaled = (float)posDepth.y * imageRect.height / sensorData.depthImageHeight;

									float xScreen = imageRect.x + xScaled;
									float yScreen = imageRect.y + imageRect.height - yScaled;

									return new Vector2(xScreen, yScreen);
								}
							}
						}
					}
				}
			}

			return Vector2.zero;
		}

		public Vector2 GetPosDepthColorOverlay(Vector3 position, Rect imageRect)
		{
			int sensorIndex = GetPrimaryBodySensorIndex();

			if (position != Vector3.zero)
			{
				// 3d position to depth
				Vector2 posDepth = MapSpacePointToDepthCoords(sensorIndex, position);
				ushort depthValue = GetDepthForPixel(sensorIndex, (int)posDepth.x, (int)posDepth.y);

				if (posDepth != Vector2.zero && depthValue > 0)
				{
					// depth pos to color pos
					Vector2 posColor = MapDepthPointToColorCoords(sensorIndex, posDepth, depthValue);

					if (posColor.x != 0f && !float.IsInfinity(posColor.x))
					{
						KinectInterop.SensorData sensorData = GetSensorData(sensorIndex);

						float xScaled = (float)posColor.x * imageRect.width / sensorData.colorImageWidth;
						float yScaled = (float)posColor.y * imageRect.height / sensorData.colorImageHeight;

						float xImage = imageRect.x + (sensorData.colorImageScale.x > 0f ? xScaled : imageRect.width - xScaled);
						float yImage = imageRect.y + (sensorData.colorImageScale.y > 0f ? yScaled : imageRect.height - yScaled);

						return new Vector2(xImage, yImage);
					}
				}
			}


			return Vector2.zero;
		}


		public Vector2 GetPosColorOverlay(Vector3 position, Rect imageRect)
		{
			int sensorIndex = GetPrimaryBodySensorIndex();

			if (position != Vector3.zero)
			{
				Vector2 posColor = MapSpacePointToColorCoords(sensorIndex, position);

				if (posColor.x != 0f && !float.IsInfinity(posColor.x))
				{
					KinectInterop.SensorData sensorData = GetSensorData(sensorIndex);

					float xScaled = (float)posColor.x * imageRect.width / sensorData.colorImageWidth;
					float yScaled = (float)posColor.y * imageRect.height / sensorData.colorImageHeight;

					float xImage = imageRect.x + (sensorData.colorImageScale.x > 0f ? xScaled : imageRect.width - xScaled);
					float yImage = imageRect.y + (sensorData.colorImageScale.y > 0f ? yScaled : imageRect.height - yScaled);

					return new Vector2(xImage, yImage);
				}
			}

			return Vector2.zero;
		}


		public Vector3 ApplyAzureFactor(Vector3 value)
		{
			value.x = value.x * kinect4AzureFactor;
			value.y = value.y * kinect4AzureFactor;
			return value;
		}

		public void CalibrateUser(ulong userId)
		{

			int userIndex = userManager.dictUserIdToIndex[userId];

			if (alTrackedBodies.Length > userIndex)
			{
				CalibrateUser(userId, userIndex, alTrackedBodies[userIndex].position);
			}
		}

		public bool IsUserExist(ulong userId)
		{
			return userManager.alUserIds.Contains(userId);
		}

		/// <summary>
		/// Determines whether the given joint of the specified user is being tracked.
		/// </summary>
		/// <returns><c>true</c> if this instance is joint tracked the specified userId joint; otherwise, <c>false</c>.</returns>
		/// <param name="userId">User ID</param>
		/// <param name="joint">Joint index</param>
		public bool IsJointOnlyTracked(ulong userId, int joint)
		{
			if (userManager.dictUserIdToIndex.ContainsKey(userId))
			{
				int index = userManager.dictUserIdToIndex[userId];

				if (index >= 0 && index < trackedBodiesCount && alTrackedBodies[index].bIsTracked)
				{
					if (joint >= 0 && joint < (int)KinectInterop.JointType.Count)
					{
						KinectInterop.JointData jointData = alTrackedBodies[index].joint[joint];

						return (int)jointData.trackingState == (int)KinectInterop.TrackingState.Tracked;
					}
				}
			}

			return false;
		}
	}
}