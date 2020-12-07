import React from 'react';
import { useAuthContext } from '../../modules/authorization/context';
import { ProfileDetailsForm, ProfileDetailsFormData } from './profileDetailsForm';
import { saveProfile } from './../../modules/profile/profileApi';
import { useErrorHandling } from '../errors/useErrorHandling';
import { SuccessAlert } from '../errors/successAlert';
import { ErrorAlert } from '../errors/errorAlert';

const ProfileDetails = () => {
    const { userProfile } = useAuthContext();
    const [showError, showSuccess, errors, asyncHandler] = useErrorHandling();

    const handleProfileSave = async (data: ProfileDetailsFormData) => {
        await asyncHandler(() => saveProfile(data));
    };

    return (
        <>
            <SuccessAlert show={showSuccess} title={"Success"} details={"Profile details updated successfully"} />
            <ErrorAlert show={showError} errors={errors} title={"Profile details save failed"} />
            <ProfileDetailsForm handleProfileSave={handleProfileSave} userProfile={userProfile} />
        </>);
};

export default ProfileDetails;
