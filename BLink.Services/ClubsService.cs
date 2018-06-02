using BLink.Core.Constants;
using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Clubs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using SixLabors.ImageSharp.Processing.Transforms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BLink.Services
{
    public class ClubsService : IClubsService
    {
        private readonly IClubsRepository _clubsRepository;
        private readonly IInvitationsRepository _invitationsRepository;
        private readonly IMembersRepository _membersRepository;
        private readonly IFileService _fileService;

        public ClubsService(
            IClubsRepository clubsRepository,
            IInvitationsRepository invitationsRepository,
            IMembersRepository membersRepository,
            IFileService fileService)
        {
            _clubsRepository = clubsRepository;
            _invitationsRepository = invitationsRepository;
            _membersRepository = membersRepository;
            _fileService = fileService;
        }

        public async Task CreateClub(CreateClubModel createClubModel)
        {
            string path = Path.Combine(
                AppConstants.DataFilesPath,
                createClubModel.Email,
                createClubModel.ClubImage.FileName);

            await _fileService.SaveImage(path, createClubModel.ClubImage);
            string thumbnailPath = _fileService.CreateThumbnailFromImage(path, createClubModel.ClubImage.FileName);
            var club = new Club
            {
                Name = createClubModel.Name,
                PhotoPath = path,
                PhotoThumbnailPath = thumbnailPath
            };

            Member member = await _membersRepository.GetMemberByEmail(createClubModel.Email);
            club.Members.Add(member);
            await _clubsRepository.CreateClub(club);
        }

        public IEnumerable<Club> GetAllClubs()
        {
            return _clubsRepository.GetClubs();
        }

        public Task<Club> GetClubById(int clubId)
        {
            return _clubsRepository.GetClubById(clubId);
        }

        public Club GetClubByName(string name)
        {
            return _clubsRepository.GetClubByName(name);
        }

        public Club GetMemberClub(string email)
        {
            return _clubsRepository
                .GetClubs(c => c.Members.Any(m => m.IdentityUser.Email == email))?
                .FirstOrDefault();
        }

        public Task InvitePlayer(Club club, Member player, CreateInvitation createInvitation)
        {
            var invitation = new Invitation
            {
                InvitedPlayer = player,
                InvitingClub = club,
                Description = createInvitation.Description,
                Status = InvitationStatus.NotReplied
            };

            return _invitationsRepository.AddInvitationAsync(invitation);
        }

        public async Task<bool> KickPlayer(int clubId, int playerId)
        {
            Club club = await GetClubById(clubId);

            if (club == null)
            {
                return false;
            }

            Member player = await _membersRepository.GetPlayerById(playerId);
            if (player == null)
            {
                return false;
            }

            bool isSuccess = true;
            if (club.Members.Contains(player))
            {
                isSuccess = club.Members.Remove(player);
            }

            return isSuccess;
        }

        public Task SaveChangesAsync()
        {
            return _clubsRepository.SaveChangesAsync();
        }

        public async Task UpdateClub(Club club, EditClub editClubModel)
        {
            if (editClubModel.ClubImage != null)
            {
                string path = Path.Combine(
                    AppConstants.DataFilesPath,
                    editClubModel.Email,
                    editClubModel.ClubImage.FileName);

                await _fileService.SaveImage(path, editClubModel.ClubImage);
                string thumbnailPath = _fileService.CreateThumbnailFromImage(path, editClubModel.ClubImage.FileName);
                club.PhotoPath = path;
                club.PhotoThumbnailPath = thumbnailPath;
            }

            club.Name = editClubModel.Name;
            _clubsRepository.UpdateClub(club);
        }
    }
}
