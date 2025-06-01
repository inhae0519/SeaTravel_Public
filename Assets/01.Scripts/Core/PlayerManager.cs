using UnityEngine;

public enum PlayerStatEnum
{
    Health,
    Energy,
    Hunger,
    Thirst,
    Oxygen
}

public class PlayerManager : MonoSingleton<PlayerManager>
{
    private Transform _playerTrm = null;
    public Transform PlayerTrm
    {
        get
        {
            if (_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return _playerTrm;
        }
    }

    private Player _player;
    public Player Player
    {
        get
        {
            if (_player == null)
            {
                _player = PlayerTrm.GetComponent<Player>();
            }

            return _player;
        }
    }

    public void StopCamRotate(bool isStop)
    {
        Player.PlayerCam.RotateStop(isStop);
    }

    public void PlayerInputOnOff(bool isTrue)
    {
        Player.PlayerInput.OnOffInput(isTrue);
    }

    public void PlayerInputCheck() => _player.PlayerInput.InputCheck();

    public void AddPlayerStat(StatTypeAndValue statTypeAndValue)
    {
        Player.PlayerStat.AddCurrentStat(statTypeAndValue.statType, statTypeAndValue.value);
    }
}
