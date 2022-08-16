using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using UnityEngine.UI;


public class Lobby_PlayerInformation : MonoBehaviour
{

    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool avatarReceived;

    public TMP_Text playerNameText;
    public RawImage playerIcon;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start()
    {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);


    }


    public void SetPlayerValues()
    {
        playerNameText.text = playerName;
        if (!avatarReceived)
        {
            GetPlayerIcon();
        }
    }

    void GetPlayerIcon()
    {
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);
        if (ImageID == -1)
        {
            return;
        }
    }


    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID == playerSteamID)
        {
            playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
  

        }
        else
        {
            return;
        }
    }


    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        avatarReceived = true;
        return texture;
    }

}
