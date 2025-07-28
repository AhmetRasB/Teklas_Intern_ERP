import React from "react";
import MasterLayout from "../masterLayout/MasterLayout";
import Breadcrumb from "../components/Breadcrumb";
import UserTablePreferencesLayer from "../components/UserTablePreferencesLayer";

const SettingsPage = () => {
  return (
    <>
      <MasterLayout>
        <Breadcrumb title="Settings - User Preferences" />
        <UserTablePreferencesLayer />
      </MasterLayout>
    </>
  );
};

export default SettingsPage; 